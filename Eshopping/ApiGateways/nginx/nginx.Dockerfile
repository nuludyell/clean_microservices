FROM nginx

COPY ApiGateways/nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY ApiGateways/nginx/id-local.crt /etc/nginx/id-local.eshopping.com.crt
COPY ApiGateways/nginx/id-local.key /etc/nginx/id-loca.eshopping.com.key