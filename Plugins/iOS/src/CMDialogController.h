//
//  MikiDialogController.h
//  MikiAPI_iOS_Framework
//
//  Created by 黄建豪 on 2017/9/2.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "CloudMacacaAPIController.h"
@interface CMDialogController : NSObject

-(void) SendEmail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body;


+(void) SetAlertDialogCallback :(AlertDialogCallback) cb;
+(void) SetDatePickerDialogCallback :(DatePickerDialogCallback) cb;


+(void) ShowAlertDialogForTitleWithCallback:(NSString*)title andForMsg:(NSString*)msg andForPositiveText:(NSString*)positiveText andForNegitiveText: (NSString*)negativeText;
+(void) ShowDatePickerWithCallback:(NSString*)okText andForCancel:(NSString*)cancelText;


+(void) ShowRateUsDialog;
+(void) ShowAlertDialogForTitle:(NSString*)title andForMsg:(NSString*)msg andForOkText:(NSString*)okText ;
+(void) ShowToastMessageForMsg:(NSString*)msg;
@end
