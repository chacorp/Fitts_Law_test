pairs=[x;y];

% demonstrating sampling with replacement

y_resample = datasample(pairs',length(x));
plot(x,y,'.b')
hold on;
plot(y_resample(:,1),y_resample(:,2),'.r')
xlabel('x');
ylabel('y');

% demonstrating the actual bootstrapping process

modelfun = @(b,x)b(1)*x(:,1)+b(2);  % b: parameters, x: independent variable

sampling_distribution_a=[];
sampling_distribution_b=[];

for i=1:1000
    y_resample = datasample(pairs',length(x));
    beta0 = [0,0];
    mdl = fitnlm(y_resample(:,1),y_resample(:,2),modelfun,beta0);
    
    sampling_distribution_a=[sampling_distribution_a;mdl.Coefficients.Estimate(1)];
    sampling_distribution_b=[sampling_distribution_b;mdl.Coefficients.Estimate(2)];        
end

figure;
hist(sampling_distribution_a);
title('sampling distribution of a');
figure;
hist(sampling_distribution_b);
title('sampling distribution of b');

% model fitting with the full data

beta0 = [0,0];
mdl = fitnlm(x,y,modelfun,beta0);
a=mdl.Coefficients.Estimate(1)
b=mdl.Coefficients.Estimate(2);

% confidence interval (99%)

a_max=a+2.576 * std(sampling_distribution_a);
a_min=a-2.576 * std(sampling_distribution_a);
b_max=b+2.576 * std(sampling_distribution_b);
b_min=b-2.576 * std(sampling_distribution_b);

figure;
hist(sampling_distribution_a);
title('sampling distribution of a with CI');
xline(a_max);
xline(a_min)
figure;
hist(sampling_distribution_b);
title('sampling distribution of b with CI');
xline(b_max);
xline(b_min)