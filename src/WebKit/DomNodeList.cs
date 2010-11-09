using System;
using System.Collections;
using System.Collections.Generic;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

namespace MonoMac.WebKit {
	public partial class DomNodeList : IEnumerable<DomNode> {
		public IEnumerator<DomNode> GetEnumerator () {
			return new DomNodeListEnumerator (this);
		}

		IEnumerator IEnumerable.GetEnumerator () {
			return new DomNodeListEnumerator (this);
		}

		class DomNodeListEnumerator : IEnumerator<DomNode> {
			public DomNodeListEnumerator (DomNodeList list) {
				_list = list;
				Reset ();
			}

			public void Dispose () {
				_list = null;
			}

			public DomNode Current {
				get { return _list [_current]; }
			}

			object IEnumerator.Current {
				get { return _list [_current]; }
			}

			public bool MoveNext () {
				return ++_current < _list.Count;
			}

			public void Reset () {
				_current = -1;
			}

			DomNodeList _list;
			int _current;
		}
	}
}
