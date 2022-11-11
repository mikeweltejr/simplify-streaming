docker run -d -p $1:8000 amazon/dynamodb-local;
dotnet run $1;
