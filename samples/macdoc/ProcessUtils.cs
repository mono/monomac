//
// ProcessUtils.cs
//
// Author:
//       Jérémie Laval <jeremie.laval@xamarin.com>
//
// Copyright (c) 2012 Xamarin, Inc.

using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace macdoc
{
	public static class ProcessUtils
	{
		public static Task<int> StartProcess (ProcessStartInfo psi, TextWriter stdout, TextWriter stderr, CancellationToken cancellationToken)
		{
			var tcs = new TaskCompletionSource<int> ();
			if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested) {
				tcs.SetCanceled ();
				return tcs.Task;
			}

			psi.UseShellExecute = false;
			if (stdout != null) {
				psi.RedirectStandardOutput = true;
			}
			if (stderr != null) {
				psi.RedirectStandardError = true;
			}
			var p = Process.Start (psi);
			if (cancellationToken.CanBeCanceled)
				cancellationToken.Register (() => {
					try {
						if (!p.HasExited) {
							p.Kill ();
						}
					} catch (InvalidOperationException ex) {
						if (ex.Message.IndexOf ("already exited") < 0)
							throw;
					}
				});
			p.EnableRaisingEvents = true;
			if (psi.RedirectStandardOutput) {
				bool stdOutInitialized = false;
				p.OutputDataReceived += (sender, e) => {
					try {
						if (stdOutInitialized)
							stdout.WriteLine ();
						stdout.Write (e.Data);
						stdOutInitialized = true;
					} catch (Exception ex) {
						tcs.SetException (ex);
					}
				};
				p.BeginOutputReadLine ();
			}
			if (psi.RedirectStandardError) {
				bool stdErrInitialized = false;
				p.ErrorDataReceived += (sender, e) => {
					try {
						if (stdErrInitialized)
							stderr.WriteLine ();
						stderr.Write (e.Data);
						stdErrInitialized = true;
					} catch (Exception ex) {
						tcs.SetException (ex);
					}
				};
				p.BeginErrorReadLine ();
			}
			p.Exited += (sender, e) => tcs.SetResult (p.ExitCode);

			return tcs.Task;
		}

		public static void StartRelaunchProcess ()
		{

		}
	}
}

