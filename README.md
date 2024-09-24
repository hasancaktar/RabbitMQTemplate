Publisher/Producer ------> exhange -----> Kuyruklar --------> Cunsomer(Mesajları tüketen yer)

Direct Exchange: Producer'dan gelen mesajı istenilen bir queue'ye direkt yönlendirme yapar

Fanout Exchange: Mesajların bu excange'e bind olmuş bütün kuyruklara gönderilmesini sağlar.

Topic Exchange: Routing key'leri kullanarak mesajları kuyruklara yönlendirmek için kulanılan bir exchange'dir. Bu exchane ile routing key'in bir kısmına/formatına/yapısına/yapısındaki keylere göre yuyruklara mesaj gönderir Kuyruklar da routing key'e göre bu exchange'e abone olabilir ve sadece ilgili routing key'e göre gönderilen mesajları alabilirler.

Header Exhange: Routing key yerine header'ları kullanarak mesajları kuyruklara yönlendirmek için kullanılan exchange'dir.

Round-Rubin Dispatching: RabbitMQ Default olarak tüm cunsomerlere sırasıyla mesaj gönderir. 

Message Acknowledgement: Kuyruktaki bir mesaj cunsomer tarafından işlenip bu mesaj için yapılacak işlem bittiğindekuyruğa bildirip sonra kuyruktan silme işlemidir. Normalde RabbitMQ kuyruktaki bir mesaj cunsomer'e ulaşınca kuyruktan siliyor ama cunsomer sağlıklı bir şekilde bu mesajı aldı mı diye kontrol etmiyor. Message Acknowledgement işlemi bunu güvenli hale getiriyor. Bu özellik cunsomer'de tanımlanır. VasicConsume metodundaki autoAck parametresini false yaparak oluyor.

BasicNack: cunsomer'de bi sıkıntı olursa mesajı işlememesi istenirse kullanılır. yar kuyruktan silebilir veya geri kuyruğa ekleyebiliriz.

BasicCancel: Bit kuyruktaki tüm mesajların işlenmesini reddetme

BasicReject: Tek bir mesajın işlenmesini reddetme

Message Durability: RabbitMQ sunucusunda bir sıkıntı olursa bütün mesajların kaybolmaması için

Fair Dispatch: rabbitMQ'da tüm cunsomer'lara eşit şekilde mesajları iletebilirsiniz. bu da kuyrukta bulunana mesajların mümkün olan en adil şekilde dağıtımını sağlamak için kullanılan bir özellik. Cunsomer'lara eşit şekilde mesajların iletilmesi sistemdeki performasnı düzenli bir hale getirecektir. böylece bir cunsomer'ın diğer cunsomer'lardan daha fazla yük alması ve sistemdeki diğer cunsomer'ların kısmi aç kalması engellenmiş olur. 
BasicQos metodu ile mesajların işlenme hızını ve teslimat sırasını belirleyebiliriz. böylece fair dispatchh özelliği konfiure edilebilmektedir.
