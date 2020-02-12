using System;
using System.Collections.Generic;
using System.Text;

namespace kviz_jatek
{
    public interface ISampleService
    {
        string GetCurrentDate();
    }

    public class SampleService : ISampleService
    {
        public string GetCurrentDate() => DateTime.Now.ToLongDateString();
    }
}
