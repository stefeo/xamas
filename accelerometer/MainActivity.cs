using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using System;
using System.Threading;

using Android.Hardware;

namespace accelerometer
{
	[Activity(Label = "accelerometer", MainLauncher = true)]
	public class MainActivity : Activity , ISensorEventListener
	{
		static readonly object _syncLock = new object();
		SensorManager _sensorManager;
		TextView _sensorTextView;
		MediaPlayer _player;
		MediaPlayer _player2;
		int l = 1;
		LinearLayout layout;
		public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }
		public void OnSensorChanged(SensorEvent e)
		{
			layout = FindViewById<LinearLayout>(Resource.Id.mainLayout);
			if (e.Values[0] < 0 && Math.Abs(e.Values[2]) < 1)
			{
				if (l == 1 && _player.IsPlaying != true)
				{
					playsound(_player);
					l = -1;
				}
				layout.SetBackgroundResource(Resource.Drawable.ramlogoGreen);
				//layout.SetBackgroundColor(Android.Graphics.Color.DarkRed);
			}
			else if (e.Values[0] > 0 && Math.Abs(e.Values[2]) < 1)
			{
				if (l == -1 && _player2.IsPlaying != true) 
				{
					playsound(_player2);
					l = 1;
				}
				layout.SetBackgroundResource(Resource.Drawable.ramlogoRed);
				//layout.SetBackgroundColor(Android.Graphics.Color.DarkGreen);
			}

			lock (_syncLock)
			{
				_sensorTextView.Text = string.Format("x={0:f}, y={1:f}, z={2:f}", e.Values[0], e.Values[1], e.Values[2]);
			}
		}
		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);

			_player = MediaPlayer.Create(this, Resource.Raw.bell);
			_player2 = MediaPlayer.Create(this, Resource.Raw.correct);

			_sensorManager = (SensorManager)GetSystemService(SensorService);
			_sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
		}
		protected override void OnResume()
		{
			base.OnResume();
			_sensorManager.RegisterListener(this,
				_sensorManager.GetDefaultSensor(SensorType.Accelerometer),
				SensorDelay.Ui);
		}
		protected override void OnPause()
		{
			base.OnPause();
			_sensorManager.UnregisterListener(this);
		}
		public void playsound(MediaPlayer _playerr) 
		{
			_playerr.Start();
		}
	}
}