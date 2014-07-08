using System;
using Android.App;
using Android.Views;
using Android.Widget;

namespace BlurExample
{
	public class IssueAdapter : BaseAdapter<int>
	{
		int []items;
		Activity context;


		public IssueAdapter(Activity context, int[] items) : base() {
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position)
		{
			return position;
		}
		public override int this[int position] {  
			get { return items[position]; }
		}
		public override int Count {
			get { return items.Length; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate(Resource.Layout.issue_item, null);

			ImageView image = view.FindViewById<ImageView> (Resource.Id.imageView1);

			image.SetImageResource(items[position]); //items[position];

			return view;
		}
	}
}

