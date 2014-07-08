using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Renderscripts;
using Android.Support.V4.Widget;
using BlurExample.Helpers;
using Android.Content.Res;
using Android.Util;
using Android.Net;

namespace BlurExample
{
	[Activity (Label = "BlurExample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		//SeekBar _seekbar;
		ImageView mBlurImage;
		FrameLayout _mainLayout;

		private MyActionBarDrawerToggle m_DrawerToggle;
		private string m_DrawerTitle;
		private string m_Title;

		private DrawerLayout m_Drawer;
		private ListView m_DrawerList;
		GridView mGrid;

		int []items;
		private static readonly string[] Sections = new[]
		{
			"Library", "NewsStand", "Reader"
		};


		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mBlurImage = FindViewById<ImageView> (Resource.Id.blur_image);

			items = new int[]{Resource.Drawable.capitale,Resource.Drawable.country,Resource.Drawable.decoration,
				Resource.Drawable.gear,Resource.Drawable.home,Resource.Drawable.maison,
				Resource.Drawable.planet, Resource.Drawable.living, Resource.Drawable.voiture, Resource.Drawable.camera,
				Resource.Drawable.food, Resource.Drawable.garden, Resource.Drawable.golf, Resource.Drawable.mac,
				Resource.Drawable.motor, Resource.Drawable.msr, Resource.Drawable.week, Resource.Drawable.wired};

			//ListAdapter = new IssueAdapter(this, items);

			mGrid = FindViewById<GridView> (Resource.Id.grid);
			mGrid.Adapter = new IssueAdapter(this, items);

			var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

			if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180) {
				mGrid.SetNumColumns (2);

			} else {
				mGrid.SetNumColumns (3);
			}

			//_seekbar = FindViewById<SeekBar> (Resource.Id.seekBar1);
			//_seekbar.StopTrackingTouch += BlurImageHandler;

			this.m_Title = this.m_DrawerTitle = this.Title;

			this.m_Drawer = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			this.m_DrawerList = this.FindViewById<ListView>(Resource.Id.left_drawer);

			this.m_DrawerList.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_menu, Sections);

			this.m_DrawerList.ItemClick += (sender, args) => ListItemClicked(args.Position);


			//this.m_Drawer.SetDrawerShadow(Resource.Drawable.drawer_shadow_dark, (int)GravityFlags.Start);



			//DrawerToggle is the animation that happens with the indicator next to the actionbar
			this.m_DrawerToggle = new MyActionBarDrawerToggle(this, this.m_Drawer,
				Resource.Drawable.ic_drawer_light,
				Resource.String.drawer_open,
				Resource.String.drawer_close)
			{

			      
			};

			//this.m_DrawerToggle.DrawerSlide += (v,slideOffset) =>
			//{
				//mBlurImage.SetImageBitmap(null);
				//mBlurImage.Visibility = ViewStates.Visible;
				//Log.Info ("slide","offset :"+slideOffset.SlideOffset);
				//if (slideOffset.SlideOffset > 0.0f) {
					//DisplayBlurredImage(slideOffset.SlideOffset);
				//}
				//else {
					//ClearBlurImage();
				//}

			//};

			//Display the current fragments title and update the options menu
			this.m_DrawerToggle.DrawerClosed += (o, args) => 
			{
				//mBlurImage.SetImageResource (Resource.Drawable.capitale);
				this.ClearBlurImage();
				this.ActionBar.Title = this.m_Title;
				this.InvalidateOptionsMenu();
			};

			//this.m_DrawerToggle.DrawerSlide += (o, args) => 
			//DisplayBlurredImage (25);

			//Display the drawer title and update the options menu
			this.m_DrawerToggle.DrawerOpened += (o, args) => 
			{
				mBlurImage.SetImageBitmap(null);
				mBlurImage.Visibility = ViewStates.Visible;
				//mBlurImage.SetAlpha(80);

				DisplayBlurredImage (10);

				this.ActionBar.Title = this.m_DrawerTitle;

				this.InvalidateOptionsMenu();
			};

			//Set the drawer lister to be the toggle.
			this.m_Drawer.SetDrawerListener(this.m_DrawerToggle);



			//if first time you will want to go ahead and click first item.
			if (savedInstanceState == null)
			{
				ListItemClicked(0);
			}
				
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);
			this.ActionBar.SetHomeButtonEnabled(true);

			m_Drawer.SetScrimColor(Resource.Color.product_sheet_dialog_background);
}

		private void ListItemClicked(int position)
		{

		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
			this.m_DrawerToggle.SyncState();
		}

		public override void OnConfigurationChanged(Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			this.m_DrawerToggle.OnConfigurationChanged(newConfig);

			var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

			if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180) {
				mGrid.SetNumColumns (2);

			} else {
				mGrid.SetNumColumns (3);
			}
		}

		// Pass the event to ActionBarDrawerToggle, if it returns
		// true, then it has handled the app icon touch event
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (this.m_DrawerToggle.OnOptionsItemSelected(item))
				return true;

			return base.OnOptionsItemSelected(item);
		}


		public override bool OnPrepareOptionsMenu(IMenu menu)
		{

			var drawerOpen = this.m_Drawer.IsDrawerOpen(this.m_DrawerList);
			//when open don't show anything
			for (int i = 0; i < menu.Size(); i++)
				menu.GetItem(i).SetVisible(!drawerOpen);


			return base.OnPrepareOptionsMenu(menu);
		}

		private Bitmap MainFrameLayout(){

			_mainLayout = FindViewById<FrameLayout>(Resource.Id.framelayout);

			_mainLayout.DrawingCacheEnabled = true;

			_mainLayout.BuildDrawingCache();

			Bitmap bm = _mainLayout.DrawingCache;

			return bm;
		}

		private void BlurImageHandler (object sender, SeekBar.StopTrackingTouchEventArgs e)
		{
			int radius = e.SeekBar.Progress;
			if (radius == 0) {
				// We don't want to blur, so just load the un-altered image.
				//mBlurImage.SetImageResource (Resource.Drawable.capitale);
				this.ClearBlurImage ();
			} else {
				Log.Info ("seekbar","radius :"+radius);
				DisplayBlurredImage (radius);
			}
		}

		private void DisplayBlurredImage (float radius)
		{
			//_seekbar.StopTrackingTouch -= BlurImageHandler;
			//_seekbar.Enabled = false;

			//ShowIndeterminateProgressDialog ();
	Task.Factory.StartNew (() => {
				Bitmap bmp = CreateBlurredImage (radius);
				return bmp;
			})
				.ContinueWith (task => {
					Bitmap bmp = task.Result;
					mBlurImage.Visibility = ViewStates.Visible;
					mBlurImage.SetImageBitmap (bmp);
					//_seekbar.StopTrackingTouch += BlurImageHandler;
					//_seekbar.Enabled = true;
					//DismissIndeterminateProgressDialog ();
				}, TaskScheduler.FromCurrentSynchronizationContext ());
		}

		private Bitmap CreateBlurredImage (float radius)
		{
			// Load a clean bitmap and work from that.
			Bitmap originalBitmap = MainFrameLayout (); //BitmapFactory.DecodeResource (Resources, Resource.Drawable.capitale);

			int width = (int)Math.Round (originalBitmap.Width * 0.4f);
			int height = (int)Math.Round (originalBitmap.Height * 0.4f);

			Bitmap inputBitmap = Bitmap.CreateScaledBitmap(originalBitmap, width, height, true);


			// Create another bitmap that will hold the results of the filter.
			Bitmap blurredBitmap;
			blurredBitmap = Bitmap.CreateBitmap (inputBitmap);

			// Create the Renderscript instance that will do the work.
			RenderScript rs = RenderScript.Create (this);

			// Allocate memory for Renderscript to work with
			Allocation input = Allocation.CreateFromBitmap (rs, inputBitmap, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
			Allocation output = Allocation.CreateTyped (rs, input.Type);

			//Allocation output = Allocation.CreateFromBitmap(rs, blurredBitmap);

			// Load up an instance of the specific script that we want to use.
			ScriptIntrinsicBlur script = ScriptIntrinsicBlur.Create (rs, Element.U8_4 (rs));
			script.SetInput (input);

			// Set the blur radius
			script.SetRadius (radius);

			// Start the ScriptIntrinisicBlur
			script.ForEach (output);

			// Copy the output to the blurred bitmap
			output.CopyTo (blurredBitmap);

			return blurredBitmap;
		}

		public void ClearBlurImage() {

			mBlurImage.Visibility = ViewStates.Gone;
			mBlurImage.SetImageBitmap(null);
		}
	}
}


