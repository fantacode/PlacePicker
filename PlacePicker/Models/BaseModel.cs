using System;
namespace PlacePicker.Models
{
    public class BaseModel<T>
    {
        public T Data { get; set; }

        public Status Status { get; set; }
    }
}
