# API Reference - SEComparer

{% code overflow="wrap" %}
```csharp
public class SEComparer : IEqualityComparer<KyaniteExpression>
```
{% endcode %}

A semantic equality comparer, implementing `IEqualityComparer`.

***

### Methods

{% code overflow="wrap" %}
```csharp
public bool Equals(KyaniteExpression? x, KyaniteExpression? y) 
```
{% endcode %}

Returns `true` if two expressions are semantically equal.

***

{% code overflow="wrap" %}
```csharp
public int GetHashCode([DisallowNull] KyaniteExpression obj)
```
{% endcode %}

Semantic hashes an expression.
