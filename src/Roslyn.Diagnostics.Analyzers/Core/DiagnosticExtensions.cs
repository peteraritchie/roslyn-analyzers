﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Analyzer.Utilities.Extensions;
using Microsoft.CodeAnalysis;

namespace Roslyn.Diagnostics.Analyzers
{
    public static class DiagnosticExtensions
    {
        public static Diagnostic CreateDiagnostic(
            this SyntaxReference syntaxReference,
            DiagnosticDescriptor rule,
            CancellationToken cancellationToken,
            params object[] args)
            => syntaxReference.GetSyntax(cancellationToken).CreateDiagnostic(rule, args);

        public static Diagnostic CreateDiagnostic(
            this SyntaxReference syntaxReference,
            DiagnosticDescriptor rule,
            ImmutableDictionary<string, string?>? properties,
            CancellationToken cancellationToken,
            params object[] args)
            => syntaxReference.GetSyntax(cancellationToken).CreateDiagnostic(rule, properties, args);

        public static Diagnostic CreateDiagnostic(
            this IEnumerable<SyntaxReference> syntaxReferences,
            DiagnosticDescriptor rule,
            ImmutableDictionary<string, string?>? properties,
            CancellationToken cancellationToken,
            params object[] args)
            => syntaxReferences.Select(s => s.GetSyntax(cancellationToken).GetLocation()).CreateDiagnostic(rule, properties, args);
    }
}
