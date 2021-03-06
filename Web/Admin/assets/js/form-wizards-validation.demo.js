/*
Template Name: Color Dotpay.Admin - Responsive Dotpay.Admin Dashboard Template build with Twitter Bootstrap 3.3.2
Version: 1.6.0
Author: Sean Ngu
Website: http://www.seantheme.com/color-admin-v1.6/admin/
*/

var handleBootstrapWizardsValidation = function() {
	"use strict";
	$("#wizard").bwizard({ validating: function (e, ui) { 
	        if (ui.index == 0) {
	            // step-1 validation
                if (false === $('form[name="form-wizard"]').parsley().validate('wizard-step-1')) {
                    return false;
                }
	        } else if (ui.index == 1) {
	            // step-2 validation
                if (false === $('form[name="form-wizard"]').parsley().validate('wizard-step-2')) {
                    return false;
                }
	        } else if (ui.index == 2) {
	            // step-3 validation
                if (false === $('form[name="form-wizard"]').parsley().validate('wizard-step-3')) {
                    return false;
                }
	        }
	    } 
	});
};

var FormWizardValidation = function () {
	"use strict";
    return {
        //main function
        init: function () {
            $.getScript('assets/plugins/parsley/dist/parsley.js').done(function() {
                $.getScript('assets/plugins/bootstrap-wizard/js/bwizard.js').done(function() {
                    handleBootstrapWizardsValidation();
                });
            });
        }
    };
}();