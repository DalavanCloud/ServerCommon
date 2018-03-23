﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.Services.Validation
{
    /// <summary>
    /// Status of the scan/sign operations
    /// </summary>
    public class ScanOperationState
    {
        /// <summary>
        /// The database-mastered identifier for this operation.
        /// </summary>
        public long Key { get; set; }

        /// <summary>
        /// Validation ID for which this operation is performed.
        /// </summary>
        public Guid PackageValidationKey { get; set; }

        /// <summary>
        /// The type of the operation performed.
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// State of the scan.
        /// </summary>
        public int ScanState { get; set; }

        /// <summary>
        /// Index of the attempt to perform an operation for current Validation.
        /// Used to prevent concurrent submission attempts.
        /// </summary>
        public int AttemptIndex { get; set; }

        /// <summary>
        /// Time when the validator detected operation request.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Time when the operation actually started.
        /// </summary>
        public DateTime? StartedAt{ get; set; }

        /// <summary>
        /// Time when the operation came to a terminal state.
        /// </summary>
        public DateTime? FinishedAt { get; set; }
        
        /// <summary>
        /// The url pointing to an operation output blob.
        /// </summary>
        public string ResultUrl { get; set; }

        /// <summary>
        /// Third party operation ID.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// Used for optimistic concurrency when updating statuses.
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
