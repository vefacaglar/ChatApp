﻿namespace ChatApp.Domain
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
