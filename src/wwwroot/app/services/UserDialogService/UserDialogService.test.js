import UserDialogService from './UserDialogService.js';

describe('UserDialogService testing: ', () => {
   let service = null;
   let $qMock = {
      defer:   () => $qMock,
      resolve: () => $qMock,
      reject:  () => $qMock
   };

   let mockTranslate = {
      instant  : jasmine.createSpy()
   };

   let mockValidationService = {
      instant : jasmine.createSpy()
   };

   let mockFoundationApi = {
      closeActiveElements : jasmine.createSpy()
   };

   let mockModalFactory = {
      activate: jasmine.createSpy()
   };

   let notificationSpy = jasmine.createSpy();
   let mockNotificationFactory = function mockNotificationFactory() { // eslint-disable-line func-style
      this.addNotification = notificationSpy;
   };

   beforeEach(() => {
      service = new UserDialogService($qMock,
                                      mockTranslate,
                                      mockValidationService,
                                      mockFoundationApi,
                                      mockModalFactory,
                                      mockNotificationFactory);
   });

   it('confirm not to be undefined or null', () => {
      expect(service.confirm).not.toBeUndefined();
      expect(service.confirm).not.toBeNull();
   });

   it('tracks that the confirm was called', () => {
      spyOn(service, 'confirm');
      service.confirm();
      expect(service.confirm).toHaveBeenCalled();
   });

   it('tracks that the confirm was called with expected argument', () => {
      spyOn(service, 'confirm');
      service.confirm('text');
      expect(service.confirm).toHaveBeenCalledWith('text');
   });

   it('notification not to be undefined or null', () => {
      expect(service.notification).not.toBeUndefined();
      expect(service.notification).not.toBeNull();
   });

   it('tracks that the notification was called', () => {
      spyOn(service, 'notification');
      service.notification();
      expect(service.notification).toHaveBeenCalled();
   });

   it('tracks that the notification was called with expected arguments', () => {
      spyOn(service, 'notification');
      service.notification('text', 'type');
      expect(service.notification).toHaveBeenCalledWith('text', 'type');
   });

   it('dialog not to be undefined or null', () => {
      expect(service.dialog).not.toBeUndefined();
      expect(service.dialog).not.toBeNull();
   });

   it('tracks that the notification was called', () => {
      spyOn(service, 'dialog');
      service.dialog();
      expect(service.dialog).toHaveBeenCalled();
   });

   it('tracks that the dialog was called with expected arguments', () => {
      spyOn(service, 'dialog');
      service.dialog('header', 'content', [{},{}], {});
      expect(service.dialog).toHaveBeenCalledWith('header', 'content', [{},{}], {});
   });

   it('tracks that the addNotification was called with expected arguments when call default type', () => {
      service.notification('text');
      expect(notificationSpy).toHaveBeenCalledWith({
         title:   mockTranslate.instant('DIALOG_SERVICE.NOTIF'),
         content: 'text',
         color:   'info'
      });
   });

   it('tracks that the addNotification was called with expected arguments when call notification', () => {
      service.notification('text', 'notification');
      expect(notificationSpy).toHaveBeenCalledWith({
         title:   mockTranslate.instant('DIALOG_SERVICE.NOTIF'),
         content: 'text',
         color:   'info'
      });
   });

   it('tracks that the addNotification was called with expected arguments when call success', () => {
      service.notification('text', 'success');
      expect(notificationSpy).toHaveBeenCalledWith({
         title: mockTranslate.instant('DIALOG_SERVICE.SUCCESS'),
         content: 'text',
         color: 'success',
         autoclose: '3000'
      });
   });

   it('tracks that the addNotification was called with expected arguments when call error', () => {
      service.notification('text', 'error');
      expect(notificationSpy).toHaveBeenCalledWith({
         title: mockTranslate.instant('DIALOG_SERVICE.ERROR'),
         content: 'text',
         color: 'alert'
      });
   });

   it('tracks that the addNotification was called with expected arguments when call warning', () => {
      service.notification('text', 'warning');
      expect(notificationSpy).toHaveBeenCalledWith({
         title: mockTranslate.instant('DIALOG_SERVICE.WARN'),
         content: 'text',
         color: 'warning',
         autoclose: '5000'
      });
   });
});
