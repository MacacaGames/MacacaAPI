//
//  LocalNotifications.h
//  CloudMacacaIOSNative
//
//  Created by Miki on 2018/1/17.
//  Copyright © 2018年 黄建豪. All rights reserved.
//

#import <Foundation/Foundation.h>

struct NotificationActionStruct {
    char* identifier;
    char* title;
    bool foreground;
    char* gameObject;
    char* handlerMethod;
};

struct NotificationStruct {
    int identifier;
    char* message;
    NSTimeInterval delay;
    NSCalendarUnit repeat;
    char* category;
    bool sound;
    char* soundName;
    int actionCount;
    struct NotificationActionStruct action1;
    struct NotificationActionStruct action2;
    struct NotificationActionStruct action3;
    struct NotificationActionStruct action4;
};

#ifdef __cplusplus
extern "C" {
#endif
    
    void scheduleNotification(struct NotificationStruct *notifStruct);
    void cancelNotification(int identifier);
    void cancelAllNotifications();
    
#ifdef __cplusplus
}
#endif
