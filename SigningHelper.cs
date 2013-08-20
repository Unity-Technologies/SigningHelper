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
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace SigningHelper
{
	public static class SigningHelper
	{
		private static readonly string kSignatureExtension = ".signature";
		public static string GetSignatureFile (string file)
		{
			return file + kSignatureExtension;
		}

		public static void Sign (string file, string keyFile)
		{
			using (var provider = new RSACryptoServiceProvider ())
			{
				provider.ImportCspBlob (File.ReadAllBytes (keyFile));
				using (var stream = new FileStream (file, FileMode.Open, FileAccess.Read))
					using (var sha1 = new SHA1CryptoServiceProvider ())
						File.WriteAllBytes (GetSignatureFile (file), provider.SignData (stream, sha1));
			}
		}

		public static bool VerifySignature (string file, string publicKeyFile)
		{
			try {
				using (var provider = new RSACryptoServiceProvider ())
				{
					byte[] blob = File.ReadAllBytes (publicKeyFile);
					try {
					
						provider.ImportCspBlob (blob);
					} catch (CryptographicException) {
						// The sn utility prepends a 12-byte header to exported public keys
						// which .NET is apparently incapable of ignoring
						provider.ImportCspBlob (blob.Skip (12).ToArray ());
					}
					using (var sha1 = new SHA1CryptoServiceProvider ())
						return provider.VerifyData (File.ReadAllBytes (file), sha1, File.ReadAllBytes (GetSignatureFile (file)));
				}
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
			return false;
		}
	}
}

