﻿using System;
using System.Collections.Generic;

namespace Project2_WebAPI.Models
{
    public partial class JobTelemetry
    {
        public int Id { get; set; }
        public string? ProccesId { get; set; }
        public string? JobId { get; set; }
        public string? QueueId { get; set; }
        public string? StepDescription { get; set; }
        public int? HumanTime { get; set; }
        public string? UniqueReference { get; set; }
        public string? UniqueReferenceType { get; set; }
        public string? BusinessFunction { get; set; }
        public string? Geography { get; set; }
        public bool? ExcludeFromTimeSaving { get; set; }
        public string? AdditionalInfo { get; set; }
        public DateTime EntryDate { get; set; }
        public int ProjectId { get; set; }
        public int ClientId { get; set; }
        public DateTime Timestamp { get; set; }
        public double TimeSaved { get; set; }
        public double CostSaved { get; set; }
    }
}
