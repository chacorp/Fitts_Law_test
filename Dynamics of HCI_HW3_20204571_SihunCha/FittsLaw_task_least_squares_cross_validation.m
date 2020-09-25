% 
indices = crossvalind('Kfold',length(x),3); % 10 fold cross validation indices

% 배열 (?)
mae_all=[];

% i는 1 ~ 10 반복
for i = 1:3
    % 선택(?)
    test = (indices == i); 
    
    % 제외(?)
    train = ~test;
    
    mdl = fitnlm(x(train),y(train),modelfun,beta0);
    a = mdl.Coefficients.Estimate(1);
    b = mdl.Coefficients.Estimate(2);
    
    y_prediction = a * x(test) + b;

    mae_all = [mae_all;mean(abs(y_prediction-y(test)))];
    
end

mean(mae_all)


