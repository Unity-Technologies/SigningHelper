//
// SigningHelper.cs
//
// Author:
//       Levi Bard <levi@unity3d.com>
//
// Copyright (c) 2013 Unity Technologies
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using NUnit.Framework;

using SH=global::SigningHelper.SigningHelper;

namespace SigningHelper.Test
{
	[TestFixture()]
	public class SigningHelper
	{
		static readonly string kGood = "../../good.snk";
		static readonly string kGoodPublic = "../../good.pub";
		static readonly string kBad = "../../bad.snk";
		static readonly string kBadPublic = "../../bad.pub";
		static readonly string kContent = "The essential Saltes of Animals may be so prepared and preserved, that an ingenious Man may have the whole Ark of Noah in his own Studie, and raise the fine Shape of an Animal out of its Ashes at his Pleasure; and by the lyke Method from the essential Saltes of humane Dust, a Philosopher may, without any criminal Necromancy, call up the Shape of any dead Ancestour from the Dust whereinto his Bodie has been incinerated.";

		[Test()]
		public void TestSigningAndVerification ()
		{
			using (var temp = new Tempfile ())
			{
				File.WriteAllText (temp.Name, kContent);
				SH.Sign (temp.Name, kGood);

				// Verify valid signatures
				Assert.IsTrue (SH.VerifySignature (temp.Name, kGood));
//				Assert.IsTrue (SH.VerifySignature (temp.Name, kGoodPublic));

				// Verify invalid signatures
				Assert.IsFalse (SH.VerifySignature (temp.Name, kBad));
//				Assert.IsFalse (SH.VerifySignature (temp.Name, kBadPublic));

				// Verify changed content => invalid signatures
				File.WriteAllText (temp.Name, kContent+kContent);
				Assert.IsFalse (SH.VerifySignature (temp.Name, kGood));
//				Assert.IsFalse (SH.VerifySignature (temp.Name, kGoodPublic));
				Assert.IsFalse (SH.VerifySignature (temp.Name, kBad));
//				Assert.IsFalse (SH.VerifySignature (temp.Name, kBadPublic));
			}
		}
	}

	class Tempfile: IDisposable
	{
		public string Name{ get; private set; }

		public Tempfile ()
		{
			Name = Path.GetTempFileName ();
		}

		~Tempfile ()
		{
			if (!string.IsNullOrEmpty (Name))
				Dispose ();
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			if (!string.IsNullOrEmpty (Name) && File.Exists (Name))
			{
				try
				{
					File.Delete (Name);
					Name = null;
				} catch { }
			}
		}

		#endregion
	}
}

