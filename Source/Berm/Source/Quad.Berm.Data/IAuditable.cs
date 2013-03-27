namespace Quad.Berm.Data
{
    using System;

    public interface IAuditable
    {
        DateTime Created { get; set; }

        DateTime? Modified { get; set; }
    }
}