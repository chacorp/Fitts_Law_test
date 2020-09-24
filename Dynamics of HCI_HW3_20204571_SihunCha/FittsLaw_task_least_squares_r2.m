%% Obtain R2

SStot=sum((y-mean(y)).^2);
SSres=sum((y-y_prediction).^2);

R2=1-SSres/SStot

%% adjusted R2

n=length(x);
p=1;
R2_adj=1-(1-R2^2)*(n-1)/(n-p-1)
