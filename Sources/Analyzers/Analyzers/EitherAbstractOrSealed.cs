﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;


namespace Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EitherAbstractOrSealed : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SealedOrAbstract";
        private static readonly string Title = "AMAnalyzer";
        private static readonly string MessageFormat = $"If a type is not sealed, it should be either static or abstract";
        private static readonly string Description = "Unsafe type extension prevention.";
        private const string Category = "Security";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(symbolContext =>
            {
                var typeDecl = (INamedTypeSymbol)symbolContext.Symbol;
                var containingType = typeDecl.ContainingType;
                if (!typeDecl.IsAbstract && !typeDecl.IsSealed && !typeDecl.IsStatic)
                {
                    var diag = Diagnostic.Create(Rule, typeDecl.Locations.First());
                    symbolContext.ReportDiagnostic(diag);
                }
            }, SymbolKind.NamedType);
        }
    }
}
