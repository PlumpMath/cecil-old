/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Binary {

    using System;

    public class ImageFormatException : Exception {

        internal ImageFormatException () : base()
        {}

        internal ImageFormatException (string message) : base(message)
        {}

        internal ImageFormatException (string message, params string[] parameters) : base(string.Format(message, parameters))
        {}

        internal ImageFormatException (string message, Exception inner) : base(message, inner)
        {}
    }
}