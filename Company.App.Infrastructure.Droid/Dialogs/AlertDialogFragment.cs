﻿using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace Company.App.Infrastructure.Dialogs
{
    internal class AlertDialogFragment : Android.Support.V4.App.DialogFragment, IAsyncDialogFragment
    {
        private const string TitleArgumentKey = "Title";
        private const string MessageArgumentKey = "Message";
        private const string AcceptArgumentKey = "Accept";

        private readonly TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        internal static AlertDialogFragment NewInstance(string title, string message, string accept)
        {
            var fragment = new AlertDialogFragment();
            var args = new Bundle();
            args.PutString(TitleArgumentKey, title);
            args.PutString(MessageArgumentKey, message);
            args.PutString(AcceptArgumentKey, accept);
            fragment.Arguments = args;

            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var title = Arguments.GetString(TitleArgumentKey);
            var message = Arguments.GetString(MessageArgumentKey);
            var accept = Arguments.GetString(AcceptArgumentKey);

            return new AlertDialog.Builder(Activity)
                .SetTitle(title)
                .SetMessage(message)
                .SetPositiveButton(accept, OnAcceptButtonClicked)
                .Create();
        }

        public Task<bool> ShowAsync(Android.Support.V4.App.FragmentManager manager, string tag)
        {
            Show(manager, tag);

            return _taskCompletionSource.Task;
        }

        private void OnAcceptButtonClicked(object sender, DialogClickEventArgs e)
        {
            _taskCompletionSource.SetResult(true);
        }
    }
}