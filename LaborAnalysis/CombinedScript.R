combinedLaborData = read.csv("CombinedLaborData.csv")

#Remove Outliers

#Prepare Data

#Create Models
engModel <- lm(En_hours ~ Points + Devices + tot_mat + tot_sub, data = combinedLaborData)
pmModel <- lm(Pm_hours ~ Points + Devices + tot_mat + tot_sub, data = combinedLaborData)
sfModel <- lm(Sf_hours ~ Points + Devices + tot_mat + tot_sub, data = combinedLaborData)
grModel <- lm(Gr_hours ~ Points + Devices + tot_mat + tot_sub, data = combinedLaborData)
cmModel <- lm(Cm_hours ~ Points + Devices + tot_mat + tot_sub, data = combinedLaborData)

#Display Models
summary(engModel)
summary(pmModel)
summary(sfModel)
summary(grModel)
summary(cmModel)
#plot(predict(engModel), engModel$En_hours, xlab = "predicted", ylab = "actual")
#abline(a = 0, b = 1)

#par(mfrow = c(2, 2))
#plot(engModel)