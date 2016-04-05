describe('VacancyService tests for', () => {
   beforeEach(module('bot'));
   it('getVacancies not to be undefined', inject((VacancyService) => {
      expect(VacancyService.getVacancies).not.toBeUndefined();
   }));

   it('getVacancies not to be null', inject((VacancyService) => {
      expect(VacancyService.getVacancies).not.toBeNull();
   }));
});
