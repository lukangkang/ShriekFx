﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shriek.Events;
using Shriek.Storage.Mementos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shriek.Domains
{
    public abstract class AggregateRoot : IAggregateRoot, IEventProvider, IOriginator
    {
        private readonly List<Event> _changes;

        [Key]
        public int Id { get; protected set; }

        public Guid AggregateId { get; protected set; }

        public int Version { get; protected set; } = -1;
        public int EventVersion { get; protected set; }

        public AggregateRoot() : this(Guid.Empty)
        {
        }

        public AggregateRoot(Guid aggregateId)
        {
            _changes = new List<Event>();
            AggregateId = aggregateId;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as AggregateRoot;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return AggregateId.Equals(compareTo.AggregateId);
        }

        public static bool operator ==(AggregateRoot a, AggregateRoot b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(AggregateRoot a, AggregateRoot b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + AggregateId.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + AggregateId.ToString() + "]";
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
            Version = history.LastOrDefault()?.Version ?? -1;
            EventVersion = Version;
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        protected void ApplyChange(Event @event, bool isNew)
        {
            dynamic d = this;
            d.Handle((dynamic)@event);
            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public Memento GetMemento()
        {
            return new Memento() { aggregateId = AggregateId, Data = JsonConvert.SerializeObject(this), Version = 0 };
        }

        public void SetMemento(Memento memento)
        {
            var data = JObject.Parse(memento.Data);
            foreach (var t in data)
            {
                var prop = GetType().GetProperty(t.Key);
                if (prop != null && prop.CanWrite)
                {
                    var value = t.Value.ToObject(prop.PropertyType);
                    prop.SetValue(this, value);
                }
            }
        }

        public bool CanCommit => _changes.Any();
    }
}