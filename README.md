# MongoDB_OLA


## a) What is sharding in MongoDB

Sharding er MongoDB's måde at bruge horizontal skalering, ved at splitte større datasæt ind i mindre dele (kaldet *shards*), som så ligger på flere servere.

---

## b) Komponenter, der kræves for at implementere sharding i MongoDB

### **Shards**
Shards er de enheder, der gemmer selve dataene. Typisk består en shard af et replikasæt for både en højere tilgængelighed og fejltolerance. Hver shard indeholder kun en delmængde af den samlede database.

### **Konfigurationsservere (Config Servers)**
Disse servere lagrer alle metadata om klyngens struktur, såsom information om hvilke data der findes på hvilke shards. Konfigurationsserverne udgør en kritisk del af systemet og skal altid være redundant opsat (mindst tre servere).

### **MongoS Routere**
MongoS fungerer som en gateway mellem klientapplikationer og sharded cluster. Routeren bruger information fra konfigurationsserverne til at dirigere forespørgsler til de rette shards baseret på shardnøglen.

---

## c) Forklar arkitekturen for sharding i MongoDB

Et sharded cluster i MongoDB er bygget op af flere vigtige komponenter:

- **Datafordeling på tværs af flere shards**:  
  Hver shard er ansvarlig for en delmængde af hele datasættet og består typisk af et replikasæt for højere driftssikkerhed.

- **Konfigurationsservere**:  
  Disse servere indeholder al nødvendig metadata om klyngens opbygning, herunder hvilke dataområder (*chunks*) der befinder sig på hvilke shards.

- **Shardingnøgle**:  
  En bestemt nøgle vælges til at bestemme, hvordan dokumenter partitioneres på tværs af shards. Valget af shardnøgle er afgørende for en jævn fordeling og effektiv forespørgselsydelse.

- **MongoS Routere**:  
  Klientapplikationer kommunikerer ikke direkte med shards, men i stedet via MongoS, som intelligent videresender anmodninger til de relevante shards, baseret på dataens placering og shardingnøglen.

---

## d) Provide implementation of map and reduce function

```javascript
var map = function() {
  if (this.entities && this.entities.hashtags) {
    this.entities.hashtags.forEach(function(tag) {
      emit(tag.text.toLowerCase(), 1);
    });
  }
};

var reduce = function(key, values) {
  return Array.sum(values);
};
```

---

## e) Provide execution command for running MapReduce or the aggregate way of doing the same

### **Query**:
```javascript
db.hashtag_counts.find().sort({ value: -1 }).limit(10)
```

### **Aggregation**:
```javascript
db.tweets.aggregate([
  { $unwind: "$entities.hashtags" },
  { $group: { _id: { $toLower: "$entities.hashtags.text" }, count: { $sum: 1 } } },
  { $sort: { count: -1 } },
  { $limit: 10 }
])
```

---

## f) Provide top 10 recorded out of the sorted result. (hint: use sort on the result returned by MapReduce or the aggregate way of doing the same)
### Top 10 Hashtags
| #  | Hashtag       | Count |
|----|---------------|-------|
| 1  | angularjs     | 29    |
| 2  | nodejs        | 29    |
| 3  | fcblive       | 27    |
| 4  | javascript    | 22    |
| 5  | globalmoms    | 19    |
| 6  | lfc           | 19    |
| 7  | espanyolfcb   | 18    |
| 8  | webinar       | 18    |
| 9  | iwci          | 17    |
| 10 | job           | 13    |

### Top 10 Tweets
| #  | Tweet ID                         | Text |
|----|----------------------------------|------|
| 1  | `553bbecae8f1e57878b72a1c` | RT @webinara: RT: http://t.co/tgxDJSOrHb #webinar #TrueTwit #TechTip. A Node.js API development webinar:<br>https://t.co/nBjkk4MnuN |
| 2  | `553bbecae8f1e57878b72a1d` | RT @zhangtai_me: Starting Angularjs Essentials, by Rodrigo Branas http://t.co/b2HaUzVtKt |
| 3  | `553bbecae8f1e57878b72a1e` | [CALENDAR] Barça have 5 league games left, 2 #UCL semi-final games, and the Spanish Cup final: http://t.co/mWKOzNEWFo http://t.co/cyN1ZZNsSx |
| 4  | `553bbecae8f1e57878b72a1f` | RT @YAPCNA: David Golden (@xdg) talks about working with both Perl and MongoDB http://t.co/BEVQB521Al http://t.co/DallwKNcR8 |
| 5  | `553bbecae8f1e57878b72a20` | Do you like social media? If so, you'll LOVE this awesome list of movies: http://t.co/aUF2WUYudF http://t.co/T0ZtFaHoKJ |
| 6  | `553bbecae8f1e57878b72a21` | RT @serkanc: I'm going to @CraftSummit, who wants to join me? Some great speakers including @sandromancuso @alexboly @lemiorhan and @oezca… |
| 7  | `553bbecae8f1e57878b72a22` | RT @zhangtai_me: 25% done with Angularjs Essentials, by Rodrigo Branas http://t.co/wAMQJi7JoQ |
| 8  | `553bbecae8f1e57878b72a23` | Spring Integration Hazelcast Support 1.0 Milestone 1 is available http://t.co/kIu102CHKY |
| 9  | `553bbecae8f1e57878b72a24` | TEAM NEWS @MCFC are unchanged from the side that beat West Ham; Okore & Westwood replace Clark & Agbonlahor for @AVFCOfficial #MCIAVL |
| 10 | `553bbecae8f1e57878b72a25` | RT @Manjuics: Apply first: MNC looking for AngularJS in HYDERABAD, IND http://t.co/ijsnQWlgGb #job |