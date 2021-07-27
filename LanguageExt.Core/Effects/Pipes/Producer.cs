using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using LanguageExt.Effects;
using LanguageExt.Effects.Traits;
using static LanguageExt.Pipes.Proxy;
using static LanguageExt.Prelude;

namespace LanguageExt.Pipes
{
    public static class Producer
    {
        /// <summary>
        /// Monad return / pure
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> Pure<RT, OUT, R>(R value) where RT : struct, HasCancel<RT> =>
            new Pure<RT, Void, Unit, Unit, OUT, R>(value).ToProducer();
        
        /// <summary>
        /// Send a value downstream (whilst in a producer)
        /// </summary>
        /// <remarks>
        /// This is the simpler version (fewer generic arguments required) of `yield` that works
        /// for producers.  In pipes, use `yieldP`
        /// </remarks>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, Unit> yield<RT, OUT>(OUT value) where RT : struct, HasCancel<RT> =>
            respond<RT, Void, Unit, Unit, OUT>(value).ToProducer();
        
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, X, X> enumerate<RT, X>(IEnumerable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, X, X, X>(xs, Producer.Pure<RT, X, X>).ToProducer();

        [Pure, MethodImpl(Proxy.mops)]
        internal static Producer<RT, OUT, X> enumerate<RT, OUT, X>(EnumerateData<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, OUT, X, X>(xs, Producer.Pure<RT, OUT, X>).ToProducer();

        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, X> enumerate<RT, OUT, X>(IEnumerable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, OUT, X, X>(xs, Producer.Pure<RT, OUT, X>).ToProducer();
        
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, X, X> enumerate<RT, X>(IAsyncEnumerable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, X, X, X>(xs, Producer.Pure<RT, X, X>).ToProducer();

        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, X> enumerate<RT, OUT, X>(IAsyncEnumerable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, OUT, X, X>(xs, Producer.Pure<RT, OUT, X>).ToProducer();

        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, X, X> observe<RT, X>(IObservable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, X, X, X>(xs, Producer.Pure<RT, X, X>).ToProducer();

        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, X> observe<RT, OUT, X>(IObservable<X> xs)
            where RT : struct, HasCancel<RT> =>
            new Enumerate<RT, Void, Unit, Unit, OUT, X, X>(xs, Producer.Pure<RT, OUT, X>).ToProducer();
     
 
        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, R> repeatM<RT, A, R>(Aff<RT, A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, R>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, Unit> repeatM<RT, A>(Aff<RT, A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, Unit>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, R> repeatM<RT, A, R>(Eff<RT, A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, R>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, Unit> repeatM<RT, A>(Eff<RT, A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, Unit>());
        
        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, R> repeatM<RT, A, R>(Aff<A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, R>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, Unit> repeatM<RT, A>(Aff<A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, Unit>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, R> repeatM<RT, A, R>(Eff<A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, R>());

        /// <summary>
        /// Repeat a monadic action indefinitely, yielding each result
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, A, Unit> repeatM<RT, A>(Eff<A> ma) where RT : struct, HasCancel<RT> =>
            Proxy.compose(lift<RT, A, A>(ma), Proxy.cat<RT, A, Unit>());

        
        /// <summary>
        /// Lift the IO monad into the Producer monad transformer (a specialism of the Proxy monad transformer)
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> lift<RT, OUT, R>(Eff<R> ma) where RT : struct, HasCancel<RT> =>
            lift<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift the IO monad into the Producer monad transformer (a specialism of the Proxy monad transformer)
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> lift<RT, OUT, R>(Aff<R> ma) where RT : struct, HasCancel<RT> =>
            lift<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift the IO monad into the Producer monad transformer (a specialism of the Proxy monad transformer)
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> lift<RT, OUT, R>(Eff<RT, R> ma) where RT : struct, HasCancel<RT> =>
            lift<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift the IO monad into the Producer monad transformer (a specialism of the Proxy monad transformer)
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> lift<RT, OUT, R>(Aff<RT, R> ma) where RT : struct, HasCancel<RT> =>
            lift<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();
        
        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Aff<R> ma) 
            where RT : struct, HasCancel<RT>
            where R : IDisposable =>
            use<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Eff<R> ma) 
            where RT : struct, HasCancel<RT>
            where R : IDisposable =>
            use<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Aff<RT, R> ma) 
            where RT : struct, HasCancel<RT> 
            where R : IDisposable =>
            use<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Eff<RT, R> ma) 
            where RT : struct, HasCancel<RT> 
            where R : IDisposable =>
            use<RT, Void, Unit, Unit, OUT, R>(ma).ToProducer();
        
        
        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Aff<R> ma, Func<R, Unit> dispose) 
            where RT : struct, HasCancel<RT> =>
            use<RT, Void, Unit, Unit, OUT, R>(ma, dispose).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Eff<R> ma, Func<R, Unit> dispose) 
            where RT : struct, HasCancel<RT> =>
            use<RT, Void, Unit, Unit, OUT, R>(ma, dispose).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Aff<RT, R> ma, Func<R, Unit> dispose) 
            where RT : struct, HasCancel<RT> =>
            use<RT, Void, Unit, Unit, OUT, R>(ma, dispose).ToProducer();

        /// <summary>
        /// Lift am IO monad into the `Proxy` monad transformer
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, R> use<RT, OUT, R>(Eff<RT, R> ma, Func<R, Unit> dispose) 
            where RT : struct, HasCancel<RT> =>
            use<RT, Void, Unit, Unit, OUT, R>(ma, dispose).ToProducer();        

        /// <summary>
        /// Release a previously used resource
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, Unit> release<RT, OUT, R>(R dispose) 
            where RT : struct, HasCancel<RT> =>
            Proxy.release<RT, Void, Unit, Unit, OUT, R>(dispose).ToProducer();

        /// <summary>
        /// Creates a non-yielding producer that returns the result of running either the left or right effect
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, Either<A, B>> sequence<RT, OUT, A, B>(Either<Effect<RT, A>, Effect<RT, B>> ms) where RT : struct, HasCancel<RT> =>
            Producer.lift<RT, OUT, Either<A, B>>(
                ms.Match(
                    Left: l => l.RunEffect().Map(Left<A, B>),
                    Right: r => r.RunEffect().Map(Right<A, B>)));

        /// <summary>
        /// Creates a that yields the result of running either the left or right effect
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, Either<A, B>, Unit> sequence<RT, A, B>(Either<Effect<RT, A>, Effect<RT, B>> ms) where RT : struct, HasCancel<RT> =>
            from r in sequence<RT, Either<A, B>, A, B>(ms)
            from _ in Producer.yield<RT, Either<A, B>>(r)
            select unit;

        /// <summary>
        /// Creates a non-yielding producer that returns the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, (A, B)> sequence<RT, OUT, A, B>((Effect<RT, A>, Effect<RT, B>) ms) where RT : struct, HasCancel<RT> =>
            Producer.lift<RT, OUT, (A, B)>((ms.Item1.RunEffect(), ms.Item2.RunEffect()).Sequence());

        /// <summary>
        /// Creates a that yields the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, (A, B), Unit> sequence<RT, A, B>((Effect<RT, A>, Effect<RT, B>) ms) where RT : struct, HasCancel<RT> =>
            from r in sequence<RT, (A, B), A, B>(ms)
            from _ in Producer.yield<RT, (A, B)>(r)
            select unit;

        /// <summary>
        /// Creates a non-yielding producer that returns the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, (A, B, C)> sequence<RT, OUT, A, B, C>((Effect<RT, A>, Effect<RT, B>, Effect<RT, C>) ms) where RT : struct, HasCancel<RT> =>
            Producer.lift<RT, OUT, (A, B, C)>((ms.Item1.RunEffect(), ms.Item2.RunEffect(), ms.Item3.RunEffect()).Sequence());

        /// <summary>
        /// Creates a that yields the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, (A, B, C), Unit> sequence<RT, A, B, C>((Effect<RT, A>, Effect<RT, B>, Effect<RT, C>) ms) where RT : struct, HasCancel<RT> =>
            from r in sequence<RT, (A, B, C), A, B, C>(ms)
            from _ in Producer.yield<RT, (A, B, C)>(r)
            select unit;

        /// <summary>
        /// Creates a non-yielding producer that returns the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, OUT, (A, B, C, D)> sequence<RT, OUT, A, B, C, D>((Effect<RT, A>, Effect<RT, B>, Effect<RT, C>, Effect<RT, D>) ms) where RT : struct, HasCancel<RT> =>
            Producer.lift<RT, OUT, (A, B, C, D)>((ms.Item1.RunEffect(), ms.Item2.RunEffect(), ms.Item3.RunEffect(), ms.Item4.RunEffect()).Sequence());

        /// <summary>
        /// Creates a that yields the result of the effects
        /// </summary>
        [Pure, MethodImpl(Proxy.mops)]
        public static Producer<RT, (A, B, C, D), Unit> sequence<RT, A, B, C, D>((Effect<RT, A>, Effect<RT, B>, Effect<RT, C>, Effect<RT, D>) ms) where RT : struct, HasCancel<RT> =>
            from r in sequence<RT, (A, B, C, D), A, B, C, D>(ms)
            from _ in Producer.yield<RT, (A, B, C, D)>(r)
            select unit;      
    }
}
