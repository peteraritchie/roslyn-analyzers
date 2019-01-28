﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Linq;
using Analyzer.Utilities;
using Analyzer.Utilities.Extensions;
using Microsoft.NetFramework.Analyzers.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.NetFramework.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public sealed class DoNotCatchCorruptedStateExceptionsAnalyzer : DoNotCatchGeneralUnlessRethrownAnalyzer
    {
        internal const string RuleId = "CA2153";

        private static readonly LocalizableString s_localizableTitle = new LocalizableResourceString(nameof(MicrosoftNetFrameworkAnalyzersResources.DoNotCatchCorruptedStateExceptions), MicrosoftNetFrameworkAnalyzersResources.ResourceManager, typeof(MicrosoftNetFrameworkAnalyzersResources));
        private static readonly LocalizableString s_localizableMessage = new LocalizableResourceString(nameof(MicrosoftNetFrameworkAnalyzersResources.DoNotCatchCorruptedStateExceptionsMessage), MicrosoftNetFrameworkAnalyzersResources.ResourceManager, typeof(MicrosoftNetFrameworkAnalyzersResources));
        private static readonly LocalizableString s_localizableDescription = new LocalizableResourceString(nameof(MicrosoftNetFrameworkAnalyzersResources.DoNotCatchCorruptedStateExceptionsDescription), MicrosoftNetFrameworkAnalyzersResources.ResourceManager, typeof(MicrosoftNetFrameworkAnalyzersResources));

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(RuleId,
                                                                             s_localizableTitle,
                                                                             s_localizableMessage,
                                                                             DiagnosticCategory.Security,
                                                                             DiagnosticHelpers.DefaultDiagnosticSeverity,
                                                                             isEnabledByDefault: DiagnosticHelpers.EnabledByDefaultIfNotBuildingVSIX,
                                                                             description: s_localizableDescription,
                                                                             helpLinkUri: "http://aka.ms/CA2153",
                                                                             customTags: WellKnownDiagnosticTags.Telemetry);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public DoNotCatchCorruptedStateExceptionsAnalyzer() : base(false)
        {
        }

        protected override bool ShouldCheckCompilationUnit(Compilation compilation)
        {
            return SecurityTypes.HandleProcessCorruptedStateExceptionsAttribute(compilation) != null;
        }

        protected override bool ShouldCheckMethod(Compilation compilation, IMethodSymbol method)
        {
            var enablingAttribute = SecurityTypes.HandleProcessCorruptedStateExceptionsAttribute(compilation);
            return method.GetAttributes().Any(attribute => attribute.AttributeClass.Equals(enablingAttribute));
        }

        protected override Diagnostic CreateDiagnostic(IMethodSymbol containingMethod, SyntaxNode catchNode)
        {
            return catchNode.CreateDiagnostic(Rule, containingMethod.ToDisplayString());
        }
    }
}