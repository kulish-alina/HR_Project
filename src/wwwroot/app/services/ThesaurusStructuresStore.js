const THESAURUS_STRUCTURES = {
   'countries' :  {
      thesaurusName : 'THESAURUSES.COUNTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',   validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'socialnetworks' : {
      thesaurusName : 'THESAURUSES.SOCIALS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'name',      type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'imagePath', label : 'image',     type : 'img' },
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'languages' : {
      thesaurusName : 'THESAURUSES.LANGUAGES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'locations' : {
      thesaurusName : 'THESAURUSES.LOCATIONS',
      fields : [
         {name : 'id',              label : 'id',                    type : ''},
         {name : 'title',           label : 'name',                  type : 'text',
          validator : 'required, maxlength=50, minlength=3'},
         {name : 'countryId',         label : 'country',               type : 'select',
          refTo : 'countries',      labelRefFieldName : 'title',     refObject : 'countryObject'},
         {name : 'state',           label : 'state',                 type : ''}
      ]
   },
   'departments' : {
      thesaurusName : 'THESAURUSES.DEPARTMENTS',
      fields : [
         {name : 'id',                    label : 'id',                    type : ''},
         {name : 'title',                 label : 'name',                  type : 'text',
          validator : 'required, maxlength=50, minlength=3'},
         {name : 'departmentGroupId',     label : 'department group',      type : 'select',
          refTo : 'departmentGroups',     labelRefFieldName : 'title',     refObject : 'departmentGroupObject'},
         {name : 'state',                 label : 'state',                 type : ''}
      ]
   },
   'departmentgroups' : {
      thesaurusName : 'THESAURUSES.DEPARTMENT_GROUPS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'industries' : {
      thesaurusName : 'THESAURUSES.INDUSTRIES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'skills' : {
      thesaurusName : 'THESAURUSES.SKILLS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'tags' : {
      thesaurusName : 'THESAURUSES.TAGS',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   },
   'stages' : {
      thesaurusName : 'THESAURUSES.STAGES',
      fields : [
         {name : 'id',        label : 'id',        type : ''},
         {name : 'title',     label : 'title',     type : 'text',    validator : 'required, maxlength=50, minlength=3'},
         {name : 'state',     label : 'state',     type : ''}
      ]
   }
};

export default THESAURUS_STRUCTURES;
