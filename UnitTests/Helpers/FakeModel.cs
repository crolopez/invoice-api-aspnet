using System;

namespace UnitTests.Helpers
{
    public class FakeModel
    {
        public string Id { get; set; }
        public int Value { get; set; }

        public FakeModel(string id, int value)
        {
            Id = id;
            Value = value;
        }
    }
}
