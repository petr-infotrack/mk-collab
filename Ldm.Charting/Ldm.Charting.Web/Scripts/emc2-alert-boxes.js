// EMC2 Alert Box jQuery
jQuery(document).ready(function($) {
    
	// Move alert if fixed to page
	$('.emc2-alert-box.fixed').prependTo('body');

	// Close button
	$('.emc2alert-close').on('click', function(){
		if( $(this).parents('.emc2-alert-box').hasClass('animate') ){
			// Animated closing
			$(this).parents('.emc2-alert-box').slideUp(250, 'swing', function(){
				$(this).remove();	
			});
		} else {
			$(this).parents('.emc2-alert-box').remove();
		} // if animated
	});
		
});


/* ********************************************************
 *	
 *	EMC2 Alert jQuery Plugin
 *	
 *	called with $('e').emc2alert( objDefaults );
 *
 *	Prepends alert box to element 'e'
 *	Accepts arguments in object form
 *
 *	Sample call:

$('body').emc2alert({ 
		title: "Your Title",	// or $('myTitleDiv').html()
		text: "Your Message",	// or $('myMsgDiv').html()
		type: "info",			// 'info', 'warning', 'error', 'success' - determines bg colors
		style: "normal",		// 'normal', 'fixed' - in page or fixed to top or bottom
		visible: true,			// true, false - hides if necessary
		position: "top",		// 'top', 'bottom' - positions box on page
		width: null,			// '100%', '960px' - specify units
		closebtn: false,		// true, false - displays close button in corner
		wpbar: false,			// true, false, 'auto' - adds top margin to avoid admin bar, with auto-detect
		animate: false			// true, false - adds open/close animation
});

 *	
 ******************************************************** */

(function($){  
	$.fn.emc2alert = function(options) {  
	
		var defaults = {  
			title: null,
			text: null,
			type: "info",  
			style: "normal",  
			visible: true,  
			position: "top",
			width: null,
			closebtn: false,
			wpbar: 'auto',
			animate: false
		};  
		
		var options = $.extend(defaults, options);  
		var output = '<div class="emc2-alert-box ';
		
		// add classes
		output += options.type + ' ';
		output += options.style + ' ';
		output += options.position + ' ';
		if( options.animate) output += 'animate ';
		
		// Add wp-bar class if needed
		if( options.wpbar == 'auto'){
			if( $('#wpadminbar').length > 0 ){ output += 'wp-bar '; }
		} else if( options.wpbar == true){ output += 'wp-bar '; }
		
		output += '" '; // end classes
		
		// Add style selectors
		output += 'style="';
		if( options.width) output += 'width:' + options.width + '; ';
		if( !options.visible){ output += 'display:none; '; }
		
		output += '"><div class="emc2-alert-wrap">'; // end style, end opening <div> tag
		
		// Add content
		if( options.closebtn) output += '<div class="emc2alert-close"></div>';
		if( options.title) output += '<h3>' + options.title + '</h3>';
		if( options.text) output += '<p>' + options.text + '</p>';
		
		output += '</div></div>'; // end wrap, end alert
		
		if( options.animate){
			return $(this).prepend( output).children('.emc2-alert-box').slideDown();
		} else {
			return $(this).prepend( output).children('.emc2-alert-box').show();
		}
	};  
})(jQuery);  