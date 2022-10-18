//
//  CMShareController.h
//  CloudMacacaIOSNative
//
//  Created by 黃建豪 on 2017/12/14.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface CMShareController : NSObject

+(void) ShareTextForTitle:(NSString*) subject andForMsg:(NSString*)message;
+(void) ShareImageForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message;
+(void)ShareGifForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message;
+(void)ShareVideoForPath:(NSString*) path andForTitle:(NSString*) subject andForMsg:(NSString*) message;

//+(void) CM_ShowWithText:(NSString*)text withFilePaths:(NSString*)filePaths withSubject:(NSString*)subject;
@end


