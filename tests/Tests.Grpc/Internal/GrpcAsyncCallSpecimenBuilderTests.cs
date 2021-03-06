﻿using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Grpc.Core;
using Kralizek.AutoFixture.Extensions.Internal;
using NUnit.Framework;

namespace Tests.Internal
{
    [TestFixture]
    public class GrpcAsyncCallSpecimenBuilderTests
    {
        [Test, CustomAutoData]
        public void Create_throws_if_arguments_are_null(GuardClauseAssertion assertion) => assertion.Verify(typeof(GrpcAsyncCallSpecimenBuilder).GetMethod(nameof(GrpcAsyncCallSpecimenBuilder.Create)));

        [Test]
        [CustomInlineAutoData(typeof(AsyncUnaryCall<HelloReply>))]
        [CustomInlineAutoData(typeof(AsyncClientStreamingCall<HelloRequest, HelloReply>))]
        [CustomInlineAutoData(typeof(AsyncServerStreamingCall<HelloReply>))]
        [CustomInlineAutoData(typeof(AsyncDuplexStreamingCall<HelloRequest, HelloReply>))]
        public void Create_can_create_calls_of_requested_type(Type type, GrpcAsyncCallSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(type, context);

            Assert.That(result, Is.InstanceOf(type));
        }

        [Test, CustomAutoData]
        public void Create_returns_no_specimen_if_type_is_unknown(GrpcAsyncCallSpecimenBuilder sut, SpecimenContext context)
        {
            var result = sut.Create(typeof(object), context);

            Assert.That(result, Is.InstanceOf<NoSpecimen>());
        }

        [Test]
        [InlineAutoData(typeof(AsyncClientStreamingCall<HelloRequest, HelloReply>))]
        [InlineAutoData(typeof(AsyncServerStreamingCall<HelloReply>))]
        [InlineAutoData(typeof(AsyncDuplexStreamingCall<HelloRequest, HelloReply>))]
        public void Create_throws_if_required_dependencies_are_missing(Type type, GrpcAsyncCallSpecimenBuilder sut, IFixture fixture)
        {
            fixture.Customizations.Add(sut);

            Assert.That(() => fixture.Create(type, new SpecimenContext(fixture)), Throws.Exception);
        }
    }
}
