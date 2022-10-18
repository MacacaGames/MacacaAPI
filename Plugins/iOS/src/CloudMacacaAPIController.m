//
//  MikiAPIController.m
//  MikiiOSNative
//
//  Created by 黄建豪 on 2017/9/3.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import "CloudMacacaAPIController.h"
#import "CMDialogController.h"
#import "CMVibrationController.h"
#import "CMShareController.h"
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>

@implementation CloudMacacaAPIController
//CMVibrationController 的接口
+(void)CM_VibrationAsLight{
    [CMVibrationController VibrationAsLight];
}
+(void)CM_VibrationAsMedium{
    [CMVibrationController VibrationAsMedium];
}
+(void)CM_VibrationAsHeavy{
    [CMVibrationController VibrationAsHeavy];
}
+(void)CM_VibrationAsSoft{
    [CMVibrationController VibrationAsSoft];
}
+(void)CM_VibrationAsRigid{
    [CMVibrationController VibrationAsRigid];
}

//CMDialogController 的接口
+(void)CM_ShowRateUsDialog{
    [CMDialogController ShowRateUsDialog];
}
+(void) CM_ShowAlertDialogForTitle:(NSString*)title andForMsg:(NSString*)msg andForOkText:(NSString*)okText {
    [CMDialogController ShowAlertDialogForTitle:title andForMsg:msg andForOkText:okText];
}
+(void)CM_ShowToastMessage:(NSString*)msg{
    [CMDialogController ShowToastMessageForMsg:msg];
}

+(void) CM_ShowAlertDialogWithCallbackForTitle:(NSString*)title andForMsg:(NSString*)msg andForPositiveText:(NSString*)positiveText andForNegitiveText: (NSString*)negativeText{
    NSLog(@"In CM_ShowAlertDialogWithCallbackForTitle");

    [CMDialogController ShowAlertDialogForTitleWithCallback:title andForMsg:msg andForPositiveText:positiveText andForNegitiveText:negativeText];
}

+(void) CM_SetAlertDialogCallback :(AlertDialogCallback) cb{
    NSLog(@"In CM_SetAlertDialogCallback");
    [CMDialogController SetAlertDialogCallback:cb];
}

+(void) CM_SetDatePickerDialogCallback:(DatePickerDialogCallback)cb{
    [CMDialogController SetDatePickerDialogCallback:cb];
}

+(void) CM_ShowDatePickerWithCallback:(NSString*)okText andForCancel:(NSString*)cancelText{
    [CMDialogController ShowDatePickerWithCallback:okText andForCancel:cancelText];
}

//CMShareController 的接口
+(void)CM_ShowShareForFilePaths:(NSString*) filePath andForSubject:(NSString*) subject andForMsg:(NSString*)msg andForTypeId:(int) typeId {
    //[CMShareController CM_ShowWithText:msg withFilePaths:filePath withSubject:subject];
    if(filePath.length > 0){
        NSLog(@"%d",typeId);
        //0 image
        //1 gif
        //2 video
        if(typeId == 0)
            [CMShareController ShareImageForPath:filePath andForTitle:subject andForMsg:msg];
        else if(typeId == 1)
            [CMShareController ShareGifForPath:filePath andForTitle:subject andForMsg:msg];
        else
            [CMShareController ShareVideoForPath:filePath andForTitle:subject andForMsg:msg];
    }
    else{
        [CMShareController ShareTextForTitle:subject andForMsg:msg];
    }
}


+(void)CM_RequestIDFA{
    if (@available(iOS 14, *)) {
        ATTrackingManagerAuthorizationStatus states = [ATTrackingManager trackingAuthorizationStatus];
        if (states == ATTrackingManagerAuthorizationStatusNotDetermined) {
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                requestCallback((int)status);
            }];
       }
    } else {

    }
}
+(int) CM_GetIDFARequestStatus{
    if (@available(iOS 14, *)) {
        ATTrackingManagerAuthorizationStatus states = [ATTrackingManager trackingAuthorizationStatus];
        return (int)states;
    }
    else {
        return 0;
    }
//    return 0;
}

RequestCallback requestCallback;
+(void) CM_SetRequestIDFACallback :(RequestCallback) cb{
    NSLog(@"In CM_SetRequestIDFACallback");
    requestCallback = cb;
}


+(void) CM_SendEmail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body{
    CMDialogController *dialogController = [[CMDialogController alloc] init];
    [dialogController SendEmail:to Subject:subject Body:body];
}
@end
