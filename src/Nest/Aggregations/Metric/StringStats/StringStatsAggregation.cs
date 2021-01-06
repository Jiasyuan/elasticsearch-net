// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Nest.Utf8Json;

namespace Nest
{
	/// <summary>
	/// A multi-value metrics aggregation that computes statistics over string values extracted from the aggregated documents.
	/// These values can be retrieved either from specific keyword fields in the documents or can be generated by a provided script.
	/// <para />
	/// Available in Elasticsearch 7.6.0+ with at least basic license level
	/// </summary>
	[InterfaceDataContract]
	[ReadAs(typeof(StringStatsAggregation))]
	public interface IStringStatsAggregation : IAggregation
	{
		/// <summary>
		/// The field to perform the aggregation on
		/// </summary>
		[DataMember(Name = "field")]
		Field Field { get; set; }

		/// <summary>
		/// A value to use for documents missing a value for the field
		/// </summary>
		[DataMember(Name = "missing")]
		object Missing { get; set; }

		/// <summary>
		/// Compute the string stats based on a script
		/// </summary>
		[DataMember(Name = "script")]
		IScript Script { get; set; }

		/// <summary>
		/// Include the probability distribution for all characters in the response.
		/// </summary>
		[DataMember(Name = "show_distribution")]
		bool? ShowDistribution { get; set; }
	}

	/// <inheritdoc cref="IStringStatsAggregation"/>
	public class StringStatsAggregation : AggregationBase, IStringStatsAggregation
	{
		internal StringStatsAggregation() { }

		public StringStatsAggregation(string name, Field field) : base(name) => Field = field;

		internal override void WrapInContainer(AggregationContainer c) => c.StringStats = this;

		/// <inheritdoc />
		public Field Field { get; set; }

		/// <inheritdoc />
		public object Missing { get; set; }

		/// <inheritdoc />
		public IScript Script { get; set; }

		/// <inheritdoc />
		public bool? ShowDistribution { get; set; }
	}

	/// <inheritdoc cref="IStringStatsAggregation"/>
	public class StringStatsAggregationDescriptor<T>
		: DescriptorBase<StringStatsAggregationDescriptor<T>, IStringStatsAggregation>, IStringStatsAggregation
		where T : class
	{
		Field IStringStatsAggregation.Field { get; set; }
		IDictionary<string, object> IAggregation.Meta { get; set; }
		object IStringStatsAggregation.Missing { get; set; }
		string IAggregation.Name { get; set; }

		IScript IStringStatsAggregation.Script { get; set; }

		bool? IStringStatsAggregation.ShowDistribution { get; set; }

		/// <inheritdoc cref="IStringStatsAggregation.Field"/>
		public StringStatsAggregationDescriptor<T> Field(Field field) => Assign(field, (a, v) => a.Field = v);

		/// <inheritdoc cref="IStringStatsAggregation.Field"/>
		public StringStatsAggregationDescriptor<T> Field<TValue>(Expression<Func<T, TValue>> field) => Assign(field, (a, v) => a.Field = v);

		/// <inheritdoc cref="IStringStatsAggregation.Script"/>
		public StringStatsAggregationDescriptor<T> Script(string script) => Assign((InlineScript)script, (a, v) => a.Script = v);

		/// <inheritdoc cref="IStringStatsAggregation.Script"/>
		public StringStatsAggregationDescriptor<T> Script(Func<ScriptDescriptor, IScript> scriptSelector) =>
			Assign(scriptSelector, (a, v) => a.Script = v?.Invoke(new ScriptDescriptor()));

		/// <inheritdoc cref="IStringStatsAggregation.Missing"/>
		public StringStatsAggregationDescriptor<T> Missing(object missing) => Assign(missing, (a, v) => a.Missing = v);

		/// <inheritdoc cref="IAggregation.Meta"/>
		public StringStatsAggregationDescriptor<T> Meta(Func<FluentDictionary<string, object>, FluentDictionary<string, object>> selector) =>
			Assign(selector, (a, v) => a.Meta = v?.Invoke(new FluentDictionary<string, object>()));

		/// <inheritdoc cref="IStringStatsAggregation.ShowDistribution"/>
		public StringStatsAggregationDescriptor<T> ShowDistribution(bool? showDistribution = true) =>
		Assign(showDistribution, (a, v) => a.ShowDistribution = v);
	}
}
