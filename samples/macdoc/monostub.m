//gcc -m32 monostub.m -o monostub -framework AppKit

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <dlfcn.h>
#include <errno.h>
#include <ctype.h>

#import <Cocoa/Cocoa.h>

#define ALSO_WITH_MONO_LIB(lib) lib,"/Library/Frameworks/Mono.framework/Versions/Current/lib/"lib

typedef int (* mono_main) (int argc, char **argv);
typedef void (* mono_free) (void *ptr);
typedef char * (* mono_get_runtime_build_info) (void);

static void
exit_with_message (char *reason, char *argv0)
{
	NSString *appName = nil;
	NSDictionary *plist = [[NSBundle mainBundle] infoDictionary];
	if (plist) {
		appName = (NSString *) [plist objectForKey:(NSString *)kCFBundleNameKey];
	}
	if (!appName) {
		appName = [[NSString stringWithUTF8String: argv0] lastPathComponent];
	}

	NSAlert *alert = [[NSAlert alloc] init];
	[alert setMessageText:[NSString stringWithFormat:@"Could not launch %@", appName]];
	NSString *fmt = @"%s\n\nPlease download and install the latest version of Mono.";
	NSString *msg = [NSString stringWithFormat:fmt, reason]; 
	[alert setInformativeText:msg];
	[alert addButtonWithTitle:@"Download Mono Framework"];
	[alert addButtonWithTitle:@"Cancel"];
	NSInteger answer = [alert runModal];
	[alert release];
	
	if (answer == NSAlertFirstButtonReturn) {
		NSString *mono_download_url = @"http://www.go-mono.com/mono-downloads/download.html";
		CFURLRef url = CFURLCreateWithString (NULL, (CFStringRef) mono_download_url, NULL);
		LSOpenCFURLRef (url, NULL);
		CFRelease (url);
	}
	exit (1);
}

static int
check_mono_version (const char *version, const char *req_version)
{
	char *req_end, *end;
	long req_val, val;
	
	while (*req_version) {
		req_val = strtol (req_version, &req_end, 10);
		if (req_version == req_end || (*req_end && *req_end != '.')) {
			fprintf (stderr, "Bad version requirement string '%s'\n", req_end);
			return FALSE;
		}
		
		req_version = req_end;
		if (*req_version)
			req_version++;
		
		val = strtol (version, &end, 10);
		if (version == end || val < req_val)
			return FALSE;
		
		if (val > req_val)
			return TRUE;
		
		if (*end != '.')
			return FALSE;	
		
		version = end + 1;
	}
	
	return TRUE;
}

static int
mkdir_with_parents (const char *dirname, int mode)
{
	char *path, *d;
	char dirsep;
	int rv;
	
	if (dirname == NULL || *dirname == '\0') {
		errno = EINVAL;
		return -1;
	}
	
	d = path = strdup (dirname);
	while (*d == '/')
		d++;
	
	while (*d != '\0') {
		while (*d && *d != '/')
			d++;
		
		dirsep = *d;
		*d = '\0';
		
		if ((rv = mkdir (path, mode)) == -1 && errno != EEXIST)
			break;
		
		*d = dirsep;
		while (*d == '/')
			d++;
	}
	
	free (path);
	
	return rv;
}

static int
redirect_io (int from_fd, const char *to_path)
{
	int err;
	int fd;
	
	if ((fd = open (to_path, O_CREAT | O_TRUNC | O_WRONLY, 0644)) == -1)
		return -1;
	
	if (dup2 (fd, from_fd) == -1) {
		err = errno;
		close (fd);
		errno = err;
		return -1;
	}
	
	return 0;
}

static void
init_logdir (const char *app)
{
	const char *env;
	size_t dirlen;
	char *path;
	
	if ((env = getenv ("MONOMAC_LOGDIR")) != NULL && *env) {
		// Redirect stdout/err to log files...
		if (mkdir_with_parents (env, 0755) == -1 && errno != EEXIST) {
			fprintf (stderr, "Could not create log directory: %s\n", strerror (errno));
			return;
		}
		
		dirlen = strlen (env);
		path = malloc (dirlen + 12);
		strcpy (path, env);
		
		if (path[dirlen - 1] != '/')
			path[dirlen++] = '/';
		
		strcpy (path + dirlen, "stdout.log");
		if (redirect_io (STDOUT_FILENO, path) == -1)
			fprintf (stderr, "Could not redirect stdout to `%s': %s\n", path, strerror (errno));
		
		strcpy (path + dirlen, "stderr.log");
		if (redirect_io (STDERR_FILENO, path) == -1)
			fprintf (stderr, "Could not redirect stderr to `%s': %s\n", path, strerror (errno));
		
		free (path);
	}
}

typedef struct _ListNode {
	struct _ListNode *next;
	char *value;
} ListNode;

static char *
decode_qstring (unsigned char **in, unsigned char qchar)
{
	unsigned char *inptr = *in;
	unsigned char *start = *in;
	char *value, *v;
	size_t len = 0;
	
	while (*inptr) {
		if (*inptr == qchar)
			break;
		
		if (*inptr == '\\') {
			if (inptr[1] == '\0')
				break;
			
			inptr++;
		}
		
		inptr++;
		len++;
	}
	
	v = value = (char *) malloc (len + 1);
	while (start < inptr) {
		if (*start == '\\')
			start++;
		
		*v++ = (char) *start++;
	}
	
	*v = '\0';
	
	if (*inptr)
		inptr++;
	
	*in = inptr;
	
	return value;
}

/* Taken from http://opensource.apple.com/source/gcc/gcc-5575.11/libiberty/strndup.c
 * Mac OS X < Lion don't natively implement strndup
 */
char *
strndup (const char *s, size_t n)
{
  char *result;
  size_t len = strlen (s);

  if (n < len)
    len = n;

  result = (char *) malloc (len + 1);
  if (!result)
    return 0;

  result[len] = '\0';
  return (char *) memcpy (result, s, len);
}

static char **
get_mono_env_options (int *count)
{
	const char *env = getenv ("MONO_ENV_OPTIONS");
	ListNode *list = NULL, *node, *tail = NULL;
	unsigned char *start, *inptr;
	char *value, **argv;
	int i, n = 0;
	
	if (env == NULL) {
		*count = 0;
		return NULL;
	}
	
	inptr = (unsigned char *) env;
	
	while (*inptr) {
		while (isblank ((int) *inptr))
			inptr++;
		
		if (*inptr == '\0')
			break;
		
		start = inptr++;
		switch (*start) {
		case '\'':
		case '"':
			value = decode_qstring (&inptr, *start);
			break;
		default:
			while (*inptr && !isblank ((int) *inptr))
				inptr++;
			
			value = strndup ((const char *) start, (size_t) (inptr - start));
			break;
		}
		
		node = (ListNode *) malloc (sizeof (ListNode));
		node->value = value;
		node->next = NULL;
		n++;
		
		if (tail != NULL)
			tail->next = node;
		else
			list = node;
		
		tail = node;
	}
	
	*count = n;
	
	if (n == 0)
		return NULL;
	
	argv = (char **) malloc (sizeof (char *) * (n + 1));
	i = 0;
	
	while (list != NULL) {
		node = list->next;
		argv[i++] = list->value;
		free (list);
		list = node;
	}
	
	argv[i] = NULL;
	
	return argv;
}

int main (int argc, char **argv)
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	NSDictionary *plist = [[NSBundle mainBundle] infoDictionary];
	NSString *resources = [[NSBundle mainBundle] resourcePath];
	NSString *exePath, *exeName = nil, *monoVersion = nil;
	char **extra_argv;
	int extra_argc;
	
	init_logdir (argv[0]);
	
	if (plist != NULL) {
		monoVersion = (NSString *) [plist objectForKey:@"MonoMinimumVersion"];
		exeName = (NSString *) [plist objectForKey:@"MonoBundleExecutable"];
	} else {
		fprintf (stderr, "Could not load Info.plist.");
	}
	
	if (!monoVersion)
		monoVersion = @"2.10.8";
	
	if (!exeName) {
		const char *basename;
		
		if (!(basename = strrchr (argv[0], '/')))
			basename = argv[0];
		else
			basename++;
		
		exeName = [NSString stringWithFormat:@"%s.exe", basename];
	}
	
	exePath = [resources stringByAppendingPathComponent: exeName];
	
	char* lib_search_paths[] = { ALSO_WITH_MONO_LIB("libmono-2.0.dylib"),
	                             ALSO_WITH_MONO_LIB("libmono-0.dylib") };
	void *libmono = NULL;
	int j = 0, search_path_count = sizeof (lib_search_paths) / sizeof (char*);
	for (j = 0; libmono == NULL && j < search_path_count; j++)
		libmono = dlopen (lib_search_paths[j], RTLD_LAZY);
	if (libmono == NULL)
		exit_with_message ("This application requires the Mono framework.", argv[0]);
	
	mono_main _mono_main = (mono_main) dlsym (libmono, "mono_main");
	if (!_mono_main) {
		fprintf (stderr, "Could not load mono_main\n");
		exit_with_message ("Failed to load the Mono framework.", argv[0]);
	}
	
	mono_free _mono_free = (mono_free) dlsym (libmono, "mono_free");
	if (!_mono_free) {
		fprintf (stderr, "Could not load mono_free\n");
		exit_with_message ("Failed to load the Mono framework.", argv[0]);
	}
	
	mono_get_runtime_build_info _mono_get_runtime_build_info = (mono_get_runtime_build_info) dlsym (libmono, "mono_get_runtime_build_info");
	if (!_mono_get_runtime_build_info) {
		fprintf (stderr, "Could not load mono_get_runtime_build_info\n");
		exit_with_message ("Failed to load the Mono framework.", argv[0]);
	}
	
	char *mono_version = _mono_get_runtime_build_info ();
	if (!check_mono_version (mono_version, [monoVersion UTF8String]))
		exit_with_message ("This application requires a newer version of the Mono framework.", argv[0]);
	
	extra_argv = get_mono_env_options (&extra_argc);
	
	char **new_argv = (char **) malloc (sizeof (char *) * (argc + extra_argc + 2));
	int i, n = 0;
	
	new_argv[n++] = argv[0];
	for (i = 0; i < extra_argc; i++)
		new_argv[n++] = extra_argv[i];
	
	new_argv[n++] = strdup ([exePath UTF8String]);
	
	for (i = 1; i < argc; i++)
		new_argv[n++] = argv[i];
	new_argv[n] = NULL;
	
	free (extra_argv);
	[pool drain];
	
	return _mono_main (argc + extra_argc + 1, new_argv);
}
