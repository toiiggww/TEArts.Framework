using System;

namespace TEArts.Framework.Collections
{
    public class GenericEventArgs<O> : EventArgs
    {
        /// <summary>
        /// 子
        /// </summary>
        public O Value1 { get; set; }
    }
    public class GenericEventArgs<O, P> : GenericEventArgs<O>
    {
        /// <summary>
        /// 丑
        /// </summary>
        public P Value2 { get; set; }
    }
    public class GenericEventArgs<O, P, Q> : GenericEventArgs<O, P>
    {
        /// <summary>
        /// 寅
        /// </summary>
        public Q Value3 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R> : GenericEventArgs<O, P, Q>
    {
        /// <summary>
        /// 卯
        /// </summary>
        public R Value4 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S> : GenericEventArgs<O, P, Q, R>
    {
        /// <summary>
        /// 辰
        /// </summary>
        public S Value5 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T> : GenericEventArgs<O, P, Q, R, S>
    {
        /// <summary>
        /// 巳
        /// </summary>
        public T Value6 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U> : GenericEventArgs<O, P, Q, R, S, T>
    {
        /// <summary>
        /// 午
        /// </summary>
        public U Value7 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U, V> : GenericEventArgs<O, P, Q, R, S, T, U>
    {
        /// <summary>
        /// 未
        /// </summary>
        public V Value8 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U, V, W> : GenericEventArgs<O, P, Q, R, S, T, U, V>
    {
        /// <summary>
        /// 申
        /// </summary>
        public W Value9 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U, V, W, X> : GenericEventArgs<O, P, Q, R, S, T, U, V, W>
    {
        /// <summary>
        /// 酉
        /// </summary>
        public X Value10 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U, V, W, X, Y> : GenericEventArgs<O, P, Q, R, S, T, U, V, W, X>
    {
        /// <summary>
        /// 戌
        /// </summary>
        public Y Value11 { get; set; }
    }
    public class GenericEventArgs<O, P, Q, R, S, T, U, V, W, X, Y, Z> : GenericEventArgs<O, P, Q, R, S, T, U, V, W, X, Y>
    {
        /// <summary>
        /// 亥
        /// </summary>
        public Z Value12 { get; set; }
    }
}
