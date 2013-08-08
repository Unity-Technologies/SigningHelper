//
// Main.cs
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
using System.Collections.Generic;
using Mono.Options;

namespace SigningHelper
{
	class MainClass
	{
		private static string KeyFile{ get; set; }
		private static string File{ get; set; }
		private static bool Sign{ get; set; }
		private static bool Verify{ get; set; }

		public static int Main (string[] args)
		{
			ParseArguments (args);
			try
			{
				if (Sign)
					SigningHelper.Sign (File, KeyFile);
				if (Verify)
				{
					if (SigningHelper.VerifySignature (File, KeyFile))
					{
						Console.WriteLine ("Signature verification succeeded for {0}:{1}", KeyFile, File);
					}
					else
					{
						Console.WriteLine ("Signature verification failed for {0}:{1}", KeyFile, File);
						return 1;
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine (e.ToString ());
				return 1;
			}

			return 0;
		}

		public static void ParseArguments(IEnumerable<string> args)
		{
			var p = new OptionSet()
				.Add("key=", "The key to use for signing/verification",
					v => (KeyFile = v))
				.Add("file=", "The file to sign/verify",
					v => (File = v))
				.Add("sign", "Sign FILE using KEY",
					v => (Sign = true))
				.Add("verify", "Verify FILE using KEY",
					v => (Verify = true));

			try
			{
				var unparsed = p.Parse(args);
				if (unparsed.Count==0 &&
				    !string.IsNullOrEmpty (KeyFile) &&
				    !string.IsNullOrEmpty (File) &&
				    (Sign || Verify)
				)
					return;

				foreach(var up in unparsed)
					Console.WriteLine("Unknown argument: "+up);
			}
			catch (OptionException)
			{
				// Don't care, print help and exit
			}

			p.WriteOptionDescriptions(Console.Out);
			Environment.Exit(1);
		}
	}
}
