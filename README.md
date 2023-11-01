# TesteUGBMVC
MVC para API
Projeto feito em 2 soluções separadas.
1 TesteUGB (API)
2 TesteUGBMVC (MVC)

- Atenção -
- para solicitar um serviço, deverá primeiro cadastrar um fornecedor (empresa)
- após o cadastro do fornecedor (empresa) deverá cadastrar o tipo de serviço que a emrpesa oferece.
- 1 empresa pode ter vários serviços diferentes.
- Na solicitação de serviço deverá escolher a empresa e os tipos de serviço que ela oferece.

- Compras -
- Ao solicitar um pedido de compra, aguarde alguns segundos, pois é enviado 1 email (fixo) para o setor de compras.
- Este email do recebedor poderá ser alterado para testes em (TesteUGBMVC/Controllers/ComprasController/Linha 89;
- Importante não solicitar compras seguidas vezes para não floodar e travar o envio de email.
