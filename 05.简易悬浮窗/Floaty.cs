using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Animation;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Astator.Platform.Droid.Script;
using Astator.Platform.Droid.Ui;
using Astator.Platform.Droid.Ui.Base;
using Astator.Platform.Droid.Ui.Controls;
using Astator.Platform.Droid.Ui.Layouts;

namespace _05.简易悬浮窗;
public class Floaty:IDisposable
{
    private readonly IWindowManager windowManager;
    private readonly FloatyWindow menuFloaty;
    private readonly ScriptFrameLayout menuRoot;
    private readonly ScriptImageButton menuBtn;

    private FloatyWindow subItemsFloaty;
    private ScriptFrameLayout subItemsRoot;
    private ScriptImageButton subMenuBtn;
    private readonly List<ScriptImageButton> subItemBtns = [];

    private readonly int size = 0;

    public Floaty(TemplateActivity activity, string menuIcon, int size, int x, int y, string[] subItemIcons, OnClickListener[] subItemOnClickListeners)
    {
        this.windowManager = activity.GetSystemService("window").JavaCast<IWindowManager>()!;
        this.size = size;

        menuRoot = activity.CreateFrameLayout(new ViewArgs { ["bg"] = "#00ffffff" });
        menuBtn = activity.CreateImageButton(new ViewArgs
        {
            ["src"] = menuIcon,
            ["w"] = size,
            ["h"] = size,
            ["radius"] = size / 2,
            ["scaleType"] = "fitCenter",
        });
        menuBtn.SetOnTouchListener(new OnTouchListener((v, e) => { return MenuBtnOnTouchListener(menuFloaty, v, e); }));
        menuRoot.AddView(menuBtn);
        menuFloaty = new FloatyWindow(menuRoot, x, y);

        InitSubItemsFloaty(activity, menuIcon, subItemIcons, subItemOnClickListeners);
    }

    private void InitSubItemsFloaty(TemplateActivity activity, string menuIcon, string[] subItemIcons, OnClickListener[] subItemOnClickListeners)
    {
        var width = Devices.Width;
        var height = Devices.Height;
        var menulayoutParams = menuRoot.LayoutParameters as WindowManagerLayoutParams;
        var onRight = menulayoutParams.X > width / 2;

        subItemsRoot =activity.CreateFrameLayout(new ViewArgs { ["bg"] = "#00ffffff" });
        subItemsRoot.SetOnTouchListener(new OnTouchListener((v, e) =>
        {
            if (e.Action == MotionEventActions.Outside)
            {
                OnClicked(subItemsRoot);
                return true;
            }
            return false;
        }));

        subMenuBtn = activity.CreateImageButton(new ViewArgs
        {
            ["src"] = menuIcon,
            ["w"] = size,
            ["h"] = size,
            ["radius"] = size / 2,
            ["scaleType"] = "fitCenter",
            ["layoutGravity"] = onRight ? "center|right" : "center|left",
        });
        subMenuBtn.SetOnClickListener(new OnClickListener(OnClicked));

        subItemsRoot.AddView(subMenuBtn);
        for (int i = 0; i < Math.Min(subItemIcons.Length, 5); i++)
        {
            var subMenuBtn = activity.CreateImageButton(new ViewArgs
            {
                ["src"] = subItemIcons[i],
                ["w"] = size,
                ["h"] = size,
                ["radius"] = size / 2,
                ["scaleType"] = "fitCenter",
                ["layoutGravity"] = onRight ? "center|right" : "center|left",
            });
            subItemsRoot.AddView(subMenuBtn);
            subItemBtns.Add(subMenuBtn);
            if (subItemOnClickListeners.Length > i && subItemOnClickListeners[i] is not null)
            {
                subMenuBtn.SetOnClickListener(subItemOnClickListeners[i]);
            }
        }

        subItemsFloaty = new FloatyWindow(subItemsRoot, flags: WindowManagerFlags.NotFocusable | WindowManagerFlags.LayoutNoLimits | WindowManagerFlags.WatchOutsideTouch);
        var subItemsLayoutParams = subItemsRoot.LayoutParameters as WindowManagerLayoutParams;
        subItemsLayoutParams.Width = Util.Dp2Px(size * 2.5);
        subItemsLayoutParams.Height = Util.Dp2Px(size * 1.25 * 2 + size);
        subItemsLayoutParams.X = menulayoutParams.X - (onRight ? subItemsLayoutParams.Width - Util.Dp2Px(size) : 0);
        subItemsLayoutParams.Y = menulayoutParams.Y - Util.Dp2Px(size * 1.25);
        windowManager.UpdateViewLayout(subItemsRoot, subItemsLayoutParams);
        subItemsRoot.Visibility = ViewStates.Gone;
    }

    private void OnClicked(View _)
    {
        var width = Devices.Width;
        var height = Devices.Height;

        var menulayoutParams = menuRoot.LayoutParameters as WindowManagerLayoutParams;
        var onRight = menulayoutParams.X > width / 2;

        if (subItemsRoot.Visibility != ViewStates.Visible)
        {
            subItemsRoot.Visibility = ViewStates.Visible;
            var subItemsLayoutParams = subItemsRoot.LayoutParameters as WindowManagerLayoutParams;
            subItemsLayoutParams.X = menulayoutParams.X - (onRight ? subItemsLayoutParams.Width - Util.Dp2Px(size) : 0);
            subItemsLayoutParams.Y = menulayoutParams.Y - Util.Dp2Px(size * 1.25);
            windowManager.UpdateViewLayout(subItemsRoot, subItemsLayoutParams);

            subMenuBtn.SetAttr("layoutGravity", onRight ? "center|right" : "center|left");
            foreach (var subItemBtn in subItemBtns)
            {
                subItemBtn.SetAttr("layoutGravity", onRight ? "center|right" : "center|left");
            }

            var animatorSet = new AnimatorSet();
            for (int i = 0; i < subItemBtns.Count; i++)
            {
                var subMenuBtn = subItemBtns[i];
                var degree = 180 / (subItemBtns.Count - 1) * (subItemBtns.Count - i - 1) * Math.PI / 180;
                var offsetX = Util.Dp2Px(size * 1.25) * Math.Sin(degree) + Util.Dp2Px(size / 3);
                var offsetY = Util.Dp2Px(size * 1.25) * Math.Cos(degree);
                var transX = ObjectAnimator.OfFloat(subMenuBtn, "translationX", 0, onRight ? -(float)offsetX : (float)offsetX);
                var transY = ObjectAnimator.OfFloat(subMenuBtn, "translationY", 0, (float)offsetY);
                var scaleX = ObjectAnimator.OfFloat(subMenuBtn, "scaleX", 0, 1);
                var scaleY = ObjectAnimator.OfFloat(subMenuBtn, "scaleY", 0, 1);
                animatorSet.Play(transX).With(transY).With(scaleX).With(scaleY);
            }
            animatorSet.SetDuration(250);
            animatorSet.Start();
        }
        else
        {
            var animatorSet = new AnimatorSet();
            for (int i = 0; i < subItemBtns.Count; i++)
            {
                var subMenuBtn = subItemBtns[i];
                var degree = 180 / (subItemBtns.Count - 1) * (subItemBtns.Count - i - 1) * Math.PI / 180;
                var offsetX = Util.Dp2Px(size) * 1.25 * Math.Sin(degree) + Util.Dp2Px(size) / 3;
                var offsetY = Util.Dp2Px(size) * 1.25 * Math.Cos(degree);

                var transX = ObjectAnimator.OfFloat(subMenuBtn, "translationX", onRight ? -(float)offsetX : (float)offsetX, 0);
                var transY = ObjectAnimator.OfFloat(subMenuBtn, "translationY", (float)offsetY, 0);
                var scaleX = ObjectAnimator.OfFloat(subMenuBtn, "scaleX", 1, 0);
                var scaleY = ObjectAnimator.OfFloat(subMenuBtn, "scaleY", 1, 0);
                animatorSet.Play(transX).With(transY).With(scaleX).With(scaleY);
            }
            animatorSet.SetDuration(250);
            animatorSet.AnimationEnd += (s, e) =>
            {
                subItemsRoot.Visibility = ViewStates.Gone;
            };
            animatorSet.Start();
        }
    }


    private float x;
    private float y;
    private bool isMoving;

    private bool MenuBtnOnTouchListener(FloatyWindow floaty, View v, MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down)
        {
            this.x = e.RawX;
            this.y = e.RawY;
        }
        else if (e.Action == MotionEventActions.Move)
        {
            var offsetX = (int)(e.RawX - this.x);
            var offsetY = (int)(e.RawY - this.y);
            if (!this.isMoving)
            {
                if (Math.Abs(offsetX) < 25 && Math.Abs(offsetY) < 25)
                {
                    return true;
                }
            }
            this.isMoving = true;
            this.x = e.RawX;
            this.y = e.RawY;

            var layoutParams = menuRoot.LayoutParameters as WindowManagerLayoutParams;

            layoutParams.X += offsetX;
            layoutParams.Y += offsetY;
            windowManager.UpdateViewLayout(menuRoot, layoutParams);
        }
        else if (e.Action == MotionEventActions.Up)
        {
            var width = Devices.Width;

            if (this.isMoving)
            {
                var layoutParams = menuRoot.LayoutParameters as WindowManagerLayoutParams;

                if (layoutParams.X < width / 2) layoutParams.X = 0;
                else layoutParams.X = width - menuRoot.Width;

                windowManager.UpdateViewLayout(menuRoot, layoutParams);
                this.isMoving = false;
            }
            else
            {
                OnClicked(v);
            }
        }
        return true;
    }

    public void Dispose()
    {
        this.menuFloaty.Dispose();
        this.subItemsFloaty.Dispose();
        GC.SuppressFinalize(this);
    }
}
