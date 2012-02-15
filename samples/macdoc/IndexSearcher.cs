using System;
using Monodoc;

namespace macdoc
{
	public class IndexSearcher
	{
		IndexReader index_reader;
		
		public IndexSearcher (IndexReader indexReader)
		{
			this.index_reader = indexReader;
		}
		
		public int FindClosest (string text)
		{
			if (index_reader == null)
				return -1;
			
			int low = 0;
			int top = index_reader.Rows-1;
			int high = top;
			int best_rate_idx = Int32.MaxValue, best_rate = -1;
			
			while (low < high){
				int mid = (high+low)/2;
				int p = mid;
				string s;
				
				for (s = index_reader.GetValue (mid); s [0] == ' ';){
					if (p == high){
						if (p == low){
							if (best_rate_idx != Int32.MaxValue)
								return best_rate_idx;
							else
								return p;
						}
						high = mid;
						break;
					}
					if (p < 0)
						return 0;
					s = index_reader.GetValue (++p);
				}
				if (s [0] == ' ')
					continue;
				int c, rate;
				c = Rate (text, s, out rate);
				if (rate > best_rate){
					best_rate = rate;
					best_rate_idx = p;
				}
				if (c == 0)
					return mid;
				if (low == high){
					if (best_rate_idx != Int32.MaxValue)
						return best_rate_idx;
					else
						return low;
				}
				if (c < 0)
					high = mid;
				else {
					if (low == mid)
						low = high;
					else
						low = mid;
				}
			}
			return high;
		}
		
		int Rate (string user_text, string db_text, out int rate)
		{
			int c = String.Compare (user_text, db_text, true);
			if (c == 0){
				rate = 0;
				return 0;
			}
			int i;
			for (i = 0; i < user_text.Length; i++){
				if (db_text [i] != user_text [i]){
					rate = i;
					return c;
				}
			}
			rate = i;
			return c;
		}
		
		public IndexEntry GetIndexEntry (int idx)
		{
			if (index_reader == null || idx >= index_reader.Rows || idx < 0)
				return null;
			return index_reader.GetIndexEntry (idx);
		}
		
		public IndexReader Index {
			get {
				return index_reader;
			}
			set {
				index_reader = value;
			}
		}
	}
}

