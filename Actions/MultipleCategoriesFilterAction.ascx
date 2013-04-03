<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Events.MultipleCategoriesFilterAction" CodeBehind="MultipleCategoriesFilterAction.ascx.cs" %>
<%@ Register TagPrefix="telerik" namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Button runat="server" ID="FilterButton" ResourceKey="FilterButton" OnClientClick="return false;" />
<div runat="server" class="events-categoryfilter-dialog">
    <telerik:RadTreeView ID="CategoriesTreeView" runat="server" 
                         CheckBoxes="True"
                         TriStateCheckBoxes="False" 
                         CheckChildNodes="True"
                         OnClientNodeChecked="OnMultipleCategoriesFilterActionClientNodeChecked" 
                         OnClientNodeClicking="OnMultipleCategoriesFilterActionClientNodeClicking" 
                         OnClientNodeClicked="OnMultipleCategoriesFilterActionClientNodeClicked" />
    <asp:Button runat="server" ID="ApplyButton" ResourceKey="ApplyButton" />
</div>

<script type="text/javascript">
    /*global jQuery, Sys */
(function($, Sys) {
    $(function () {
        $("body").click(function () {
            $('.events-categoryfilter-dialog').dialog("close");
        });

        var handleDialogClick = function (e) {
                e.stopPropagation();
            },
            initializeDialog = function () {
                var $button = $('#<%=this.FilterButton.ClientID %>'),
                    $dlg = $button.siblings('.events-categoryfilter-dialog').dialog({
                        autoOpen: false,
                        dialogClass: 'events-categoryfilter-dialog-wrap',
                        position: {
                            my: '<%=DialogPosition %>',
                            at: '<%=ButtonPosition %>',
                            of: $('#<%=this.FilterButton.ClientID %>'),
                            collision: '<%=CollisionBehavior %>'
                        }
                    });

                $dlg.click(handleDialogClick);
                $dlg.parent().appendTo($("#Form"));

                $button.click(function () {
                    if ($dlg.dialog("isOpen")) {
                        $dlg.dialog("close");
                    }
                    else {
                        $dlg.dialog("open");
                    }

                    return false;
                });
            };

        initializeDialog();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            // note that this will fire when _any_ UpdatePanel is triggered,
            // which may or may not cause an issue
            initializeDialog();
        });
    });

    function DisableEnableAll(node) {
        var treeView = node.get_treeView(),
            nodes = treeView.get_allNodes(),
            i;

        if (node.get_level() == 0 && node.get_index() == 0) {
            // the root (all categories) node is checked
            var checked = node.get_checked();
            for (i = 1; i < nodes.length; i++) {
                var attributes = nodes[i].get_attributes();
                if (nodes[i].get_nodes() != null) {
                    if (attributes.getAttribute("enabled") == "1") {
                        nodes[i].set_enabled(!checked);
                        nodes[i].set_checked(false);
                    }
                    else {
                        // node is not in the module setting filter.
                        nodes[i].set_enabled(false);
                        nodes[i].set_checked(false);
                    }
                }
            }
        }
        else {
            // check if all nodes are unchecked
            var allUnchecked = true;
            for (i = 1; i < nodes.length; i++) {
                allUnchecked = allUnchecked && !nodes[i].get_checked();
            }

            if (allUnchecked) {
                nodes[0].set_checked(true);

                for (i = 1; i < nodes.length; i++) {
                    if (nodes[i].get_nodes() != null) {
                        nodes[i].set_enabled(false);
                        nodes[i].set_checked(false);
                    }
                }
            }
        }
    }

    window.OnMultipleCategoriesFilterActionClientNodeClicked = window.OnMultipleCategoriesFilterActionClientNodeClicked || function(sender, event) {
        var node = event.get_node();
        node.set_selected(false);
        return false;
    };

    window.OnMultipleCategoriesFilterActionClientNodeClicking = window.OnMultipleCategoriesFilterActionClientNodeClicking || function(sender, event) {
        var node = event.get_node();
        node.set_checked(!node.get_checked());
        node.set_selected(false);
        DisableEnableAll(node);
        return false;
    };

    window.OnMultipleCategoriesFilterActionClientNodeChecked = window.OnMultipleCategoriesFilterActionClientNodeChecked || function(sender, event) {
        var node = event.get_node();
        DisableEnableAll(node);
    };
}(jQuery, Sys));
</script>