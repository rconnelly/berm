namespace Quad.Berm.Data
{
    using System;

    public interface IAuditable
    {
        DateTime Created { get; }

        DateTime? Modified { get; }
    }
}