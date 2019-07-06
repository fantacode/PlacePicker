using System;
using Xamarin.Forms.Internals;

namespace PlacePicker.Models
{
    [Preserve(AllMembers = true)]
    public class BaseModel<T>
    {
        public T Data { get; set; }

        public Status Status { get; set; }
    }
}
