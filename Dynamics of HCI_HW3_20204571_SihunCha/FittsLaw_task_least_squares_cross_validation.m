
indices=crossvalind('Kfold',length(x),10); % 10 fold cross validation indices

mae_all=[];
for i = 1:10
    test = (indices == i); 
    train = ~test;
    
    mdl = fitnlm(x(train),y(train),modelfun,beta0);
    y_prediction=mdl.Coefficients.Estimate(1)*x(test)+mdl.Coefficients.Estimate(2);

    mae_all=[mae_all;mean(abs(y_prediction-y(test)))];
    
end

mean(mae_all)


