docker cp ./test restful-api:/data
docker cp ./malware.txt restful-api:/data
docker cp ./t.txt restful-api:/data
curl -X POST http://localhost:3000/analyze
echo ""
