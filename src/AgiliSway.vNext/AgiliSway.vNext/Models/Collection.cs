using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgiliSway.vNext.Models
{
    public class Collection
    {
		public int CollectionId { get; set; }

		public int SubjectId { get; set; }
		public virtual Subject Subject { get; set; }

		public int? TaskId { get; set; }
		public virtual Task Task { get; set; }

		public string ExternalId { get; set; }
		public string Title { get; set; }
		public string FilePath { get; set; }
		public string FileType { get; set; }
		public string ImportPath { get; set; }
		public DateTime? Timestamp { get; set; }
		public string Notes { get; set; }
    }

	public class CollectionDataSesssion
	{
		public CollectionDataSesssion()
		{
			Calibration = new CollectionDataSet();
			DataPoints = new CollectionDataSet();
		}

		public string ExternalId { get; set; }
		public CollectionDataSet Calibration { get; set; }
		public CollectionDataSet DataPoints { get; set; }
	}

	public class CollectionDataSet
	{
		public CollectionDataSet()
		{
			PointSet = new List<CollectionDataPoint>();
		}

		public DateTime? TimestampUtc { get; set; }
		public List<CollectionDataPoint> PointSet { get; set; }
	}

	public class CollectionDataPoint
	{
		public CollectionDataPoint()
		{
			TopLeft = new CollectionValue();
			TopRight = new CollectionValue();
			BottomLeft = new CollectionValue();
			BottomRight = new CollectionValue();
		}

        public DateTime TimestampUtc { get; set; }
		public CollectionValue TopLeft { get; set; }
		public CollectionValue TopRight { get; set; }
		public CollectionValue BottomLeft { get; set; }
		public CollectionValue BottomRight { get; set; }
	}

	public class CollectionValue
	{
		public int? X { get; set; }
		public int? Y { get; set; }
		public int? Z { get; set; }
	}
}
