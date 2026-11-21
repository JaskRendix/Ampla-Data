using System;
using NUnit.Framework;

namespace AmplaData.Web.Wrappers
{
    [TestFixture]
    public class SimpleHttpContextUnitTests : TestFixture
    {
        [Test]
        public void RedirectWillChangeRequestUrl()
        {
            SimpleHttpContext context = SimpleHttpContext.Create(
                "http://localhost/Production/Index");

            Assert.That(
                context.Request.Url.ToString(),
                Is.EqualTo("http://localhost/Production/Index"));

            context.Response.Redirect(
                "http://localhost/Quality/Index");

            Assert.That(
                context.Request.Url.ToString(),
                Is.EqualTo("http://localhost/Quality/Index"));
        }

        [Test]
        public void RedirectWillChangeTakeResponseCookiesToRequest()
        {
            SimpleHttpContext context = SimpleHttpContext.Create(
                "http://localhost/Production/Index");

            Assert.That(
                context.Request.Url.ToString(),
                Is.EqualTo("http://localhost/Production/Index"));

            Assert.That(context.Request.Cookies, Is.Empty);

            context.Response.AppendCookie(
                "Test",
                "UnitTest");

            context.Response.Redirect(
                "http://localhost/Quality/Index");

            Assert.That(
                context.Request.Url.ToString(),
                Is.EqualTo("http://localhost/Quality/Index"));

            Assert.That(context.Request.Cookies, Is.Not.Empty);
            Assert.That(context.Request.GetCookie("Test"),
                Is.EqualTo("UnitTest"));
        }

        [Test]
        public void OldRequestThrowsDisposed()
        {
            SimpleHttpContext context = SimpleHttpContext.Create(
                "http://localhost/Production/Index");

            var oldRequest = context.Request;
            var oldResponse = context.Response;

            context.Response.Redirect(
                "http://localhost/Quality/Index");

            Uri url = null;

            Assert.Throws<ObjectDisposedException>(
                () => { url = oldRequest.Url; });

            Assert.That(url, Is.Null);

            Assert.Throws<ObjectDisposedException>(
                () => oldResponse.Redirect("http://localhost"));
        }

        [Test]
        public void WithSessionsDisabled()
        {
            SimpleHttpContext context =
                SimpleHttpContext.Create("http://localhost")
                                 .WithSessionsDisabled();

            Assert.That(context.Session.Enabled, Is.False);
        }
    }
}