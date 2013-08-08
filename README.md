Signing Helper
=======================

This is a small, command-line helper utility for signing and verifying
arbitrary files using .NET signing keys.

Dependencies
============

 * Mono or .NET 3.5 (or a framework version that's API-compatible with 3.5)
 * NUnit 2.4+

Usage
=====

SigningHelper command line arguments:
    --key=VALUE            The key to use for signing/verification
    --file=VALUE           The file to sign/verify
    --sign                 Sign FILE using KEY
    --verify               Verify FILE using KEY

Example: 
    SigningHelper.exe --key=mykey.snk --file=myfile --sign --verify
 * Signs myfile using mykey.snk
 * Stores the signature in myfile.signature
 * Verifies the signature against mykey.snk

You should subsequently be able to:
    SigningHelper.exe --key=mykey.pub --file=myfile --verify
Verifies that myfile:
 * Has a .signature
 * Was signed with the private component of mykey.pub
 * Has the same contents as at the time of signing

